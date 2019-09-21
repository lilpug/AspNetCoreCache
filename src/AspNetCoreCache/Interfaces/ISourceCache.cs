using System.Threading.Tasks;

namespace AspNetCoreCache.Interfaces
{
    public interface ISourceCache<ItemType>
    {
        Task<ItemType> GetCacheItem();
        Task<ItemType> GetItemFromSource();
        Task RefreshCache();
    }
}