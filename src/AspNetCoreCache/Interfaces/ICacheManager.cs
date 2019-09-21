using System.Threading.Tasks;

namespace AspNetCoreCache.Interfaces
{  
    public interface ICacheManager
    {
        Task<T> GetCacheItem<T>(string cacheName);
        Task SetUpdateCacheItem<T>(string cacheName, T item);
        Task ClearCache(string cacheName);
    }
}