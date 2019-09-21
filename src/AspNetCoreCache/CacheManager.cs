using AspNetCoreCache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace AspNetCoreCache
{
    public class CacheManager : BaseCache, ICacheManager
    {
        public CacheManager(IDistributedCache cache, DistributedCacheEntryOptions cacheOptions) : base(cache, cacheOptions)
        {
        }

        public async virtual Task<T> GetCacheItem<T>(string cacheName) => await base.GetBaseCacheItem<T>(cacheName);

        public async virtual Task SetUpdateCacheItem<T>(string cacheName, T item) => await base.SetUpdateBaseCacheItem(cacheName, item);

        public async virtual Task ClearCache(string cacheName) => await base.ClearBaseCache(cacheName);
    }
}