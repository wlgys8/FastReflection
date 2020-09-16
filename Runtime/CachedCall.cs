using System.Reflection;
using System;

namespace MS.Reflection{

    public class CachedCallFactory{

        internal static System.Type MakeCachedCallGenericType(System.Type[] arguementTypes){
            if(arguementTypes == null || arguementTypes.Length == 0){
                return typeof(CachedCall);
            }else if(arguementTypes.Length == 1){
                return typeof (CachedCall<>).MakeGenericType(arguementTypes);
            }else if(arguementTypes.Length == 2){
                return typeof (CachedCall<,>).MakeGenericType(arguementTypes);
            }else if(arguementTypes.Length == 3){
                return typeof (CachedCall<,,>).MakeGenericType(arguementTypes);
            }else if(arguementTypes.Length == 4){
                return typeof (CachedCall<,,,>).MakeGenericType(arguementTypes);
            }else{
                throw new System.ArgumentOutOfRangeException("max supported arguements lenght is 4");
            }
        }

        public static BaseCachedCall Create(object target,string func,System.Type[] arguementTypes,bool throwExceptionsIfNotFound = true){
            var methodInfo = target.GetType().GetMethod(func,arguementTypes);
            if(methodInfo == null){
                if(throwExceptionsIfNotFound){
                    throw new System.MissingMethodException($"{target.GetType()}:{func}");
                }
                return null;
            }
            if(arguementTypes == null || arguementTypes.Length == 0){
                return new CachedCall(target,methodInfo);
            }else{
                var type = MakeCachedCallGenericType(arguementTypes);
                var constructor = type.GetConstructor(new System.Type[2]{ typeof (object), typeof (MethodInfo)});
                var cachedCall = constructor.Invoke(new object[]{target,methodInfo});
                return (BaseCachedCall)cachedCall;   
            }
      
        }

        public static BaseCachedCall CreateStatic(System.Type type,string func,System.Type[] arguementTypes){
            var methodInfo = type.GetMethod(func,arguementTypes);
            if(methodInfo == null){
                throw new System.MissingMethodException($"{type}:{func}");
            }
            if(arguementTypes == null || arguementTypes.Length == 0){
                return new CachedCall(null,methodInfo);
            }else{
                var cachedCallType = MakeCachedCallGenericType(arguementTypes);
                var constructor = cachedCallType.GetConstructor(new System.Type[2]{ typeof (object), typeof (MethodInfo)});
                var cachedCall = constructor.Invoke(new object[]{null,methodInfo});
                return (BaseCachedCall)cachedCall;    
            }
               
        }
    }

    public abstract class BaseCachedCall{
        public abstract void Invoke(object[] args);
    }


    public class CachedCall:BaseCachedCall{

        private Action _action;
        public CachedCall(object target,MethodInfo method){
            _action = (Action)method.CreateDelegate(typeof(Action),target);
        }
        public override void Invoke(object[] args){
            _action();
        }
    }

    public class CachedCall<T>:BaseCachedCall{
        private Action<T> _action;
        public CachedCall(object target,MethodInfo method){
            _action = (Action<T>)method.CreateDelegate(typeof(Action<T>),target);
        }
   
        public override void Invoke(object[] args){
            _action((T)args[0]);
        }
    }

    public class CachedCall<T0,T1>:BaseCachedCall{
        private Action<T0,T1> _action;
        public CachedCall(object target,MethodInfo method){
            _action = (Action<T0,T1>)method.CreateDelegate(typeof(Action<T0,T1>),target);
        }
        public override void Invoke(object[] args){
            _action((T0)args[0],(T1)args[1]);
        }
    }


    public class CachedCall<T0,T1,T2>:BaseCachedCall{
        private Action<T0,T1,T2> _action;
        public CachedCall(object target,MethodInfo method){
            _action = (Action<T0,T1,T2>)method.CreateDelegate(typeof(Action<T0,T1,T2>),target);
        }
        public override void Invoke(object[] args){
            _action((T0)args[0],(T1)args[1],(T2)args[2]);
        }
    }

    public class CachedCall<T0,T1,T2,T3>:BaseCachedCall{
        private Action<T0,T1,T2,T3> _action;
        public CachedCall(object target,MethodInfo method){
            _action = (Action<T0,T1,T2,T3>)method.CreateDelegate(typeof(Action<T0,T1,T2,T3>),target);
        }
        public override void Invoke(object[] args){
            _action((T0)args[0],(T1)args[1],(T2)args[2],(T3)args[3]);
        }
    }



}
