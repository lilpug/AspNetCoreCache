using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreCache.UnitTests
{
    public class SingleCacheManagerTests
    {
        public class ExampleAdjustableSingleCache : SingleCacheManager<Dictionary<string, string>>
        {
            private const string CACHENAME = "ExampleCacheName";
            public ExampleAdjustableSingleCache(IDistributedCache cache) : base(CACHENAME, cache, null)
            {                
            }
        }

        private IDistributedCache Cache { get; set; }
        private Dictionary<string, string> MockDictionary { get; set; }

        private string CacheName { get; set; }

        [SetUp]
        public void Setup()
        {
            IOptions<MemoryDistributedCacheOptions> options = Options.Create(new MemoryDistributedCacheOptions());
            Cache = new MemoryDistributedCache(options);
            MockDictionary = new Dictionary<string, string>() { { "first", "time" }, { "any other", "time" } };
            CacheName = "example";
        }           

        [Test]
        public async Task GetEmptyCache()
        {
            ExampleAdjustableSingleCache cache = new ExampleAdjustableSingleCache(Cache);            
            var item = await cache.GetCacheItem();
            Assert.IsTrue(item == null);
        }

        [Test]
        public async Task SetAndGetCache()
        {
            ExampleAdjustableSingleCache cache = new ExampleAdjustableSingleCache(Cache);
            await cache.SetUpdateCacheItem(MockDictionary);            
            var item = await cache.GetCacheItem();
            Assert.IsTrue(item != null && item.ContainsKey("first") && item.ContainsKey("any other"));
        }

        [Test]
        public async Task ClearCache()
        {
            ExampleAdjustableSingleCache cache = new ExampleAdjustableSingleCache(Cache);
            await cache.SetUpdateCacheItem(MockDictionary);
            await cache.ClearCache();            
            var item = await cache.GetCacheItem();
            Assert.IsTrue(item == null);
        }
    }
}