using FruitShop.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FruitShop.Host.Data.EntityConfigurations
{
    public class ProviderEntityConfiguration : IEntityTypeConfiguration<ProviderEntity>
    {
        public void Configure(EntityTypeBuilder<ProviderEntity> builder)
        {
            builder.ToTable("Providers");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("provider_hilo")
                .IsRequired();

            builder.Property(cb => cb.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(cb => cb.Address)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
