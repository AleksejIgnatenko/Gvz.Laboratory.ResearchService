using Gvz.Laboratory.ResearchService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvz.Laboratory.ResearchService.ConfigurationsDb
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(x => x.UnitsOfMeasurement)
                .IsRequired()
                .HasMaxLength(16);
        }
    }
}
