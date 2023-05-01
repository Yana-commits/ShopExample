using FruitShop.Host.Data.Entities;
using FruitShop.Host.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace FruitShop.Host.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<FruitItemEntity> FruitItemEntities { get; set; }
        public DbSet<FruitSortEntity> FruitSortEntities { get; set; }
        public DbSet<FruitTypeEntity> FruitTypeEntities { get; set; }
        public DbSet<ProviderEntity> ProviderEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FruitItemEntityConfiguration());
            builder.ApplyConfiguration(new FruitSortEntityConfiguration());
            builder.ApplyConfiguration(new FruitTypeEntityConfiguration());
            builder.ApplyConfiguration(new ProviderEntityConfiguration());
        }
    }
}
