using FruitShop.Host.Data.Entities;
using FruitShop.Host.Data;
using FruitShop.Host.Models.Enums;

namespace FruitShop.Host.Repositories.Interfaces
{
    public interface IFruitItemRepository
    {
        Task<PaginatedItems<FruitItemEntity>> GetByPageAsync(int pageIndex, int pageSize);
        Task<int?> Add(string name, int fruitTypeId, int fruitSortId, string description,
            decimal price, int providerId, string pictureUrl);
        Task<FruitItemEntity?> GetFruitByIdAsync(int fruitId);
        Task<FruitItemsByType<FruitItemEntity>?> GetCatalogByTypeAsync(string type, int pageIndex, int pageSize);
        Task<bool> UpdateFruitItemAsync(int id, string description);

        Task<bool> DeleteFruitItemAsync(int id);
        Task<FruitItemsByType<FruitTypeEntity>> GetTypesAsync();
    }
}
