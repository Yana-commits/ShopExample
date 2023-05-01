using Microsoft.EntityFrameworkCore;
using Order.Host.Data.Entities;
using Order.Host.Data.EntityConfig;

namespace Order.Host.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<OrderEntity> Order { get; set; } = null!;
        public DbSet<OrderItemEntity> OrderItem { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new OrderEntityConfig());
            builder.ApplyConfiguration(new OrderItemEntityConfig());
        }
    }
}
