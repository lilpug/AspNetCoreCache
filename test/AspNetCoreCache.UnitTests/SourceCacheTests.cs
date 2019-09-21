using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreCache.UnitTests
{
    public class SourceCacheTests
    {
        public class ExampleSourceCache : SourceCache<Dictionary<string, string>>
        {
            private const string CACHENAME = "ExampleCacheName";
            protected int Counter { get; set; }

            public ExampleSourceCache(IDistributedCache cache) : base(CACHENAME, cache, null)
            {
                Counter = 0;
            }

            public override Task<Dictionary<string, string>> GetItemFromSource()
            {
                Counter++;
                if (Counter == 1)
                {
                    return Task.Run(() => { return new Dictionary<string, string>() { { "first", "time" } }; });
                }
                else
                {
                    return Task.Run(() => { return new Dictionary<string, string>() { { "any other", "time" } }; });
                }
            }
        }

        private MemoryDistributedCache Cache { get; set; }
        private Dictionary<string, byte[]> CacheBuffer { get; set; }

        private string CacheName { get; set; }

        [SetUp]
        public void Setup()
        {            
            IOptions<MemoryDistributedCacheOptions> options = Options.Create(new MemoryDistributedCacheOptions());
            Cache = new MemoryDistributedCache(options);
        }           

        [Test]
        public async Task GetEmptyCacheLoadFromSource()
        {
            ExampleSourceCache cache = new ExampleSourceCache(Cache);
            var item = await cache.GetCacheItem();
            Assert.IsTrue(item != null && item.ContainsKey("first") && item["first"] == "time");
        }

        [Test]
        public async Task GetEmptyCacheSecondLoadFromCache()
        {
            ExampleSourceCache cache = new ExampleSourceCache(Cache);
            var item = await cache.GetCacheItem();
            item = await cache.GetCacheItem();

            Assert.IsTrue(item != null && item.ContainsKey("first") && item["first"] == "time");
        }

        [Test]
        public async Task CacheRefreshLoadFromSource()
        {
            ExampleSourceCache cache = new ExampleSourceCache(Cache);
            var item = await cache.GetCacheItem();
            await cache.RefreshCache();
            item = await cache.GetCacheItem();

            Assert.IsTrue(item != null && item.ContainsKey("any other") && item["any other"] == "time");
        }
    }
}