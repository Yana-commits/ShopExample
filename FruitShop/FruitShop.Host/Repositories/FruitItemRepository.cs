using FruitShop.Host.Data;
using FruitShop.Host.Data.Entities;
using FruitShop.Host.Models.Enums;
using FruitShop.Host.Repositories.Interfaces;
using FruitShop.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FruitShop.Host.Repositories
{
    public class FruitItemRepository : IFruitItemRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<FruitItemRepository> _logger;

        public FruitItemRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<FruitItemRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }
        public async Task<PaginatedItems<FruitItemEntity>> GetByPageAsync(int pageIndex, int pageSize)
        {
            var totalItems = await _dbContext.FruitItemEntities
                .LongCountAsync();

            var itemsOnPage = await _dbContext.FruitItemEntities
                .Include(i => i.FruitSort)
                .Include(i => i.FruitType)
                .Include(i => i.Provider)
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItems<FruitItemEntity>() { TotalCount = totalItems, Data = itemsOnPage };
        }
        public async Task<int?> Add(string name, int fruitTypeId, int fruitSortId, string description,
            decimal price, int providerId, string pictureUrl)
        {
            var item = await _dbContext.FruitItemEntities.AddAsync(new FruitItemEntity
            {
                Name = name,
                FruitTypeId = fruitTypeId,
                FruitSortId = fruitSortId,
                Description = description,
                Price = price,
                ProviderId = providerId,
                PictureUrl = pictureUrl
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }
        public async Task<FruitItemEntity?> GetFruitByIdAsync(int fruitId)
        {
            var catalog = await _dbContext.FruitItemEntities.Where(f => f.Id == fruitId).Include(i => i.FruitSort)
                .Include(i => i.FruitType).Include(i => i.Provider).FirstOrDefaultAsync();

            if (catalog == null)
            {
                _logger.LogWarning($"Not founded product with id = {fruitId}");
                return null!;
            }

            return catalog;
        }
        public async Task<FruitItemsByType<FruitItemEntity>?> GetCatalogByTypeAsync(string type, int pageIndex, int pageSize)
        {
            var fruits = await _dbContext.FruitItemEntities
                .Where(f => f.FruitType.Type == type)
                .Include(i => i.FruitSort)
                .Include(i => i.Provider)
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            if (fruits == null)
            {
                _logger.LogWarning($"Not founded product with type = {type}");
                return null!;
            }

            return new FruitItemsByType<FruitItemEntity>() { TotalCount = fruits.Count, Data = fruits };
        }
        
        public async Task<bool> UpdateFruitItemAsync(int id, string description)
        {
            var entity = await _dbContext.FruitItemEntities.FirstOrDefaultAsync(f => f.Id == id);
            if (entity == null)
            {
                _logger.LogWarning($"Not founded product with id = {id}");
                return false;
            }

            entity!.Description = description;
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteFruitItemAsync(int id)
        {
            var entity = await _dbContext.FruitItemEntities.FirstOrDefaultAsync(f => f.Id == id);
            if (entity == null)
            {
                _logger.LogWarning($"Not founded product with id = {id}");
                return false;
            }

            _dbContext.Entry(entity).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<FruitItemsByType<FruitTypeEntity>> GetTypesAsync()
        {
            var totalCount = await _dbContext.FruitTypeEntities.Select(c => c.Type).LongCountAsync();

            var brands = await _dbContext.FruitTypeEntities.ToListAsync();

            return new FruitItemsByType<FruitTypeEntity>() { TotalCount = totalCount, Data = brands };
        }
    }
}
