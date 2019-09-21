# AspNetCoreCache

AspNetCoreCache is a ASP.NET Core cache wrapper for quick and effective cache usage. It deals with all the hard work for having sourced caches and a cache manager to make life easier.

[![The MIT License](https://img.shields.io/badge/license-MIT-orange.svg?style=flat-square&maxAge=3600)](https://raw.githubusercontent.com/lilpug/AspNetCoreCache/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/AspNetCoreCache.svg?maxAge=3600)](https://www.nuget.org/packages/AspNetCoreCache/)

## SourceCache

The SourceCache can be used when you want a single variable type to be cached and want all the process to be fully managed for you apart from its data source.

### Setup Process

You can use the SourceExample like below to add your own data source into the equation.
```c#
public class SourceExample : SourceCache<Dictionary<string,string>>
{
    private const string CACHENAMEKEY = "exampleCacheKeyName";        

    public SourceExample(IDistributedCache cache) : base(CACHENAMEKEY, cache, new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)))
    {                        
    }

    //This function allows you to use a source for the cache, such as a database call        
    public override Task<Dictionary<string, string>> GetItemFromSource()
    {            
        return Task.Run(() => { return new Dictionary<string, string>(); });
    }
}
```

#### With Dependency Injection

You can then also add your inherited class into your Startup.cs -> ConfigureServices function to add it to the Dependency Injection.
```c#
services.AddSingleton<ISourceCache<Dictionary<string, string>>, SourceExample>();
```

You can then use the DI like so.
```c#
public class ExampleController : ControllerBase
{
    private readonly ISourceCache<Dictionary<string, string>> _SourceCache;
    public ExampleController(ISourceCache<Dictionary<string,string>> sourceCache)
    {
        _SourceCache = sourceCache;
    }
}
```

#### Without Dependency Injection

You can create an instance of your class like so.
```c#
SourceExample sourceCache = new SourceExample(IDistributedMemoryExample);
```

### Example Usage

Once you have an instance of the class, you can then use the following functions.
```c#
//Pulls the item out of the cache
var item = await sourceCache.GetCacheItem();

//Refreshes the cache to load from the datasource again
await sourceCache.RefreshCache();
```


## SingleCacheManager

The SingleCacheManager can be used when you have a single type you want to cache but you want full control of the get,add/update and clear capabilities rather than it being taken care of automatically for you.

### Setup Process

You can use the SingleCacheManager like below
```c#
public class SingleCacheExample : SingleCacheManager<Dictionary<string, string>>
{
    private const string CACHENAMEKEY = "exampleCacheKeyName";

    public SingleCacheExample(IDistributedCache cache) : base(CACHENAMEKEY, cache, new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)))
    {            
    }
}
```

#### With Dependency Injection

You can then also add your inherited class into your Startup.cs -> ConfigureServices function to add it to the Dependency Injection.
```c#
services.AddSingleton<ISingleCacheManager<Dictionary<string, string>>, SingleCacheExample>();
```

You can then use the DI like so.
```c#
public class ExampleController : ControllerBase
{
    private readonly ISingleCacheManager<Dictionary<string, string>> _SingleCacheManager;
    public ExampleController(ISingleCacheManager<Dictionary<string,string>> singleCacheManager)
    {
        _SingleCacheManager = singleCacheManager;
    }
}
```

#### Without Dependency Injection

You can create an instance of your class like so.
```c#
SingleCacheExample singleCache = new SingleCacheExample(IDistributedMemoryExample);
```

### Example Usage

Once you have an instance of the class, you can then use the following functions.
```c#
//Pulls the item out of the cache
var item = await singleCache.GetCacheItem();

//Adds/Updates the cache with the supplied variable
await singleCache.SetUpdateCacheItem(newOrUpdatedVariable);

//Clears the cache down 
await singleCache.ClearCache();
```

## CacheManager

The CacheManager can be used when you have multiple types you want to cache but you want full control of the get, add/update and clear capabilities rather than it being taken care of automatically for you.

### Setup Process

You can use the CacheManager like below
```c#
CacheManager cacheManager = new CacheManager(IDistributedMemoryExample, MemoryOptionsExample);
```

#### With Dependency Injection

You can then also add the CacheManager into your Startup.cs -> ConfigureServices function to add it to Dependency Injection.
```c#
services.AddSingleton<ICacheManager, CacheManager>();   
```

You can then use the DI like so.
```c#
public class ExampleController : ControllerBase
{
    private readonly ICacheManager _CacheManager;
    public ExampleController(ICacheManager cacheManager)
    {
        _CacheManager = cacheManager;
    }
}
```

#### Without Dependency Injection

You can create an instance of your class like so.
```c#
CacheManager cacheManager = new CacheManager(IDistributedMemoryExample, MemoryOptionsExample);
```

### Example Usage

Once you have an instance of the class, you can then use the following functions.
```c#
//Pulls the item out of the cache as the specified variable type with the provided key
var item = await cacheManager.GetCacheItem<TypeExpectedOut>("CacheKeyNameHere");

//Adds/Updates the cache with the supplied variable for the specified key
await cacheManager.SetUpdateCacheItem("CacheKeyNameHere", newOrUpdatedVariable);

//Clears the cache down for that particular key
await cacheManager.ClearCache("CacheKeyNameHere");
```

## Copyright and License
Copyright &copy; 2019 David Whitehead

This project is licensed under the MIT License.