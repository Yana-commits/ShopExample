using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Host.Data.Entities;

namespace Order.Host.Data.EntityConfig
{
    public class OrderItemEntityConfig : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.ToTable("OrderItem");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id).IsRequired().UseHiLo("order_item_hilo");
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        }
    }
}
