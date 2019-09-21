using AspNetCoreCache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace AspNetCoreCache
{ 
    public abstract class SourceCache<ItemType> : BaseCache, ISourceCache<ItemType>
    {
        protected readonly string _CacheName;

        public SourceCache(string cacheName, IDistributedCache cache, DistributedCacheEntryOptions cacheOptions) : base(cache, cacheOptions)
        {
            _CacheName = cacheName;
        }

        public async virtual Task<ItemType> GetCacheItem()
        {
            //Attempts to pull it from the cache
            var item = await base.GetBaseCacheItem<ItemType>(_CacheName);

            //If the cache is empty then it loads it from the source and sets the cache
            if (item == null)
            {
                item = await GetItemFromSource();
                await SetUpdateBaseCacheItem(_CacheName, item);
            }
            return item;
        }

        public abstract Task<ItemType> GetItemFromSource();

        public async virtual Task RefreshCache()
        {
            ItemType item = await GetItemFromSource();
            await SetUpdateBaseCacheItem(_CacheName, item);
        }
    }
}