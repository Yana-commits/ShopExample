using FruitShop.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Repositories.Interfaces;

namespace Order.Host.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderItemRepository> _logger;

        public OrderItemRepository(
            IDbContextWrapper<ApplicationDbContext> wrapper,
            ILogger<OrderItemRepository> logger)
        {
            _context = wrapper.DbContext;
            _logger = logger;
        }

        public async Task<int?> Add(string name, decimal cost, int orderId)
        {
            var entity = await _context.OrderItem.AddAsync(new OrderItemEntity()
            {
                Name = name,
                Cost = cost
            });

            await _context.SaveChangesAsync();
            return entity?.Entity.Id;
        }

        public async Task<OrderItemEntity?> Get(int id)
        {
            return await _context.OrderItem.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await Get(id);
            if (entity == null)
            {
                _logger.LogError("Failed_delete");
                return false;
            }

            _context.Entry(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
