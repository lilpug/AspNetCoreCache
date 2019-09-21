using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreCache
{
    public abstract class BaseCache
    {
        protected readonly IDistributedCache _Cache;
        protected readonly DistributedCacheEntryOptions _CacheOptions;

        public BaseCache(IDistributedCache cache, DistributedCacheEntryOptions cacheOptions)
        {
            _Cache = cache;
            _CacheOptions = cacheOptions ?? new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
        }

        protected async virtual Task<T> GetBaseCacheItem<T>(string cacheName)
        {
            //Reads it from the cache
            byte[] encodedMapper = await _Cache.GetAsync(cacheName);

            //Checks if what we got back is empty or not
            if (encodedMapper != null && encodedMapper.Length > 0)
            {
                string json = Encoding.UTF8.GetString(encodedMapper);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }

            return default(T);
        }

        protected async virtual Task SetUpdateBaseCacheItem<T>(string cacheName, T item)
        {
            string freshMapper = JsonConvert.SerializeObject(item);
            byte[] encodedFreshMapper = Encoding.UTF8.GetBytes(freshMapper);
            await _Cache.SetAsync(cacheName, encodedFreshMapper, _CacheOptions);
        }

        protected async virtual Task ClearBaseCache(string cacheName)
        {
            await _Cache.RemoveAsync(cacheName);
        }

    }
}