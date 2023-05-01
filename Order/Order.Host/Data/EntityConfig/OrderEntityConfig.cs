using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Host.Data.Entities;

namespace Order.Host.Data.EntityConfig
{
    public class OrderEntityConfig : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id).IsRequired().UseHiLo("order_hilo");

            builder.HasMany(m => m.Items).WithOne();
        }
    }
}
