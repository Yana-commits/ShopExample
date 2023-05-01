using FruitShop.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FruitShop.Host.Data.EntityConfigurations
{
    public class FruitItemEntityConfiguration : IEntityTypeConfiguration<FruitItemEntity>
    {
        public void Configure(EntityTypeBuilder<FruitItemEntity> builder)
        {
            builder.ToTable("FruitItems");

            builder.Property(ci => ci.Id)
                .UseHiLo("fruit_item_hilo")
                .IsRequired();

            builder.Property(ci => ci.Name)
               .IsRequired(true)
               .HasMaxLength(100);

            builder.HasOne(ci => ci.FruitType)
            .WithMany()
            .HasForeignKey(ci => ci.FruitTypeId);

            builder.HasOne(ci => ci.FruitSort)
            .WithMany()
            .HasForeignKey(ci => ci.FruitSortId);

            builder.Property(ci => ci.Description)
              .IsRequired(true)
              .HasMaxLength(100);

            builder.Property(ci => ci.Price)
             .IsRequired(true);

            builder.HasOne(ci => ci.Provider)
            .WithMany()
            .HasForeignKey(ci => ci.ProviderId);

            builder.Property(ci => ci.PictureUrl)
               .IsRequired(true)
               .HasMaxLength(100);
        }
    }
}
