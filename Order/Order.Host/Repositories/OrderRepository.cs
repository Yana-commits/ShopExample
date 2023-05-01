using FruitShop.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Models.Response;
using Order.Host.Repositories.Interfaces;

namespace Order.Host.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(
            IDbContextWrapper<ApplicationDbContext> wrapper,
            ILogger<OrderRepository> logger)
        {
            _context = wrapper.DbContext;
            _logger = logger;
        }

        public async Task<int?> Add(string userId, decimal totalCost, IEnumerable<OrderItemEntity> items)
        {
            var entity = await _context.Order.AddAsync(new OrderEntity()
            {
                UserId = userId,
                TotalCost = totalCost,
                Date = DateTime.UtcNow,
                Status = "Created"
            });

            await _context.OrderItem.AddRangeAsync(items.Select(s => new OrderItemEntity()
            {
                Name = s.Name,
                Cost = s.Cost,
            }).ToList());

            await _context.SaveChangesAsync();
            return entity.Entity.Id;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await Get(id);
            if (entity == null)
            {
                _logger.LogError("Failed_DElete");
                return false;
            }

            _context.Entry(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OrderEntity?> Get(int id)
        {
            return await _context.Order.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<BasketResponse<OrderEntity>?> GetUserOrders(string userId, int pageIndex, int pageSize)
        {
            IQueryable<OrderEntity> query = _context.Order;

            query = query.Where(w => w.UserId == userId);

            var totalItems = await query.LongCountAsync();
            var itemsOnPage = await query.OrderBy(o => o.Date)
                .Include(i => i.Items)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new BasketResponse<OrderEntity>()
            {
                TotalCost = totalItems,
                BasketList = itemsOnPage
            };
        }

        public async Task<bool> UpdateStatus(int id, string status)
        {
            var entity = await Get(id);
            if (entity == null)
            {
                _logger.LogError("Failed_Update");
                return false;
            }

            entity.Status = status;
            _context.Entry(entity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
