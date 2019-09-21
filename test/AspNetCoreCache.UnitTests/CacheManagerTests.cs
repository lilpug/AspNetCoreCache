using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreCache.UnitTests
{
    public class CacheManagerTests
    {
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
            CacheManager cache = new CacheManager(Cache, null);                        
            var item = await cache.GetCacheItem<Dictionary<string, string>>(CacheName);
            Assert.IsTrue(item == null);
        }

        [Test]
        public async Task SetAndGetCache()
        {

            CacheManager cache = new CacheManager(Cache, null);
            await cache.SetUpdateCacheItem(CacheName, MockDictionary);
            var item = await cache.GetCacheItem<Dictionary<string, string>>(CacheName);
            Assert.IsTrue(item != null && item.ContainsKey("first") && item.ContainsKey("any other"));
        }

        [Test]
        public async Task ClearCache()
        {
            CacheManager cache = new CacheManager(Cache, null);
            await cache.SetUpdateCacheItem(CacheName, MockDictionary);
            await cache.ClearCache(CacheName);            
            var item = await cache.GetCacheItem<Dictionary<string, string>>(CacheName);
            Assert.IsTrue(item == null);
        }
    }
}
