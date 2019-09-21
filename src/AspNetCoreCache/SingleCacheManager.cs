using AspNetCoreCache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace AspNetCoreCache
{
    public abstract class SingleCacheManager<ItemType> : BaseCache, ISingleCacheManager<ItemType>
    {
        protected readonly string _CacheName;

        public SingleCacheManager(string cacheName, IDistributedCache cache, DistributedCacheEntryOptions cacheOptions) : base(cache, cacheOptions)
        {
            _CacheName = cacheName;
        }

        public async virtual Task<ItemType> GetCacheItem()
        {
            return await GetBaseCacheItem<ItemType>(_CacheName);
        }

        public async virtual Task SetUpdateCacheItem(ItemType item) => await base.SetUpdateBaseCacheItem(_CacheName, item);

        public async virtual Task ClearCache() => await base.ClearBaseCache(_CacheName);
    }
}