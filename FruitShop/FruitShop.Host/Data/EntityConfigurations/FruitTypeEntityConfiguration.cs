using FruitShop.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FruitShop.Host.Data.EntityConfigurations
{
    public class FruitTypeEntityConfiguration : IEntityTypeConfiguration<FruitTypeEntity>
    {
        public void Configure(EntityTypeBuilder<FruitTypeEntity> builder)
        {
            builder.ToTable("FruitTypes");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("fruit_type_hilo")
                .IsRequired();

            builder.Property(cb => cb.Type)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
