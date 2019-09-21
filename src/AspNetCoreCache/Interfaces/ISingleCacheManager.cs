using System.Threading.Tasks;

namespace AspNetCoreCache.Interfaces
{   
    public interface ISingleCacheManager<ItemType>
    {
        Task<ItemType> GetCacheItem();
        Task SetUpdateCacheItem(ItemType item);
        Task ClearCache();
    }
}