using FruitShop.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FruitShop.Host.Data.EntityConfigurations
{
    public class FruitSortEntityConfiguration : IEntityTypeConfiguration<FruitSortEntity>
    {
        public void Configure(EntityTypeBuilder<FruitSortEntity> builder)
        {
            builder.ToTable("FruitSorts");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("fruit_sort_hilo")
                .IsRequired();

            builder.Property(cb => cb.Sort)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
