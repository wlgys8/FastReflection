# Unity FastReflection

Convert `MethodInfo` to `Delegate` to reduce the invoke cost.


# How to use


1. Create a cached call of target's func method with parameter types.
``` csharp
static CachedCallFactory.Create(object target,string func,System.Type[] arguementTypes) => BaseCachedCall

```

2. Call Invoke of BaseCachedCall

```csharp

BaseCachedCall.Invoke(object[] args) => void

```




Examples:


```csharp

public class TestObject{

    public void Run(){
    }
}

//....

var obj = new TestObject();
//Create a cached call of obj.Run(parameters)
var cachedCall = CachedCallFactory.Create(obj,"Run",new Type[]{});

//Invoke the method
cachedCall.Invoke(null);

```

# IL2CPP AOT Issue

If you create a cachedCall with ValueType parameters, it may caused AOT Issue. 

More Info: 
https://stackoverflow.com/questions/56183606/invoke-generic-method-via-reflection-in-c-sharp-il2cpp-on-ios

To resolve this issue, we must use `CacheCallArguementsTypeRegister.Ensure` to let compiler generate AOT code.

for example:

```csharp

//These codes can be put anywhere of your project, alse no need to be ran. It is only for compiler.
private static void Register(){
    //Support create cachedCalls with one int paramster
    CacheCallArguementsTypeRegister.EnsureCall<int>();

    //Support create cachedCalls with bool & int parameters
    CacheCallArguementsTypeRegister.EnsureCall<bool,int>();
}


```

# Benchmark

