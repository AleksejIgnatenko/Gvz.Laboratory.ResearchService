using Gvz.Laboratory.ResearchService.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ResearchService.ConfigurationsDb
{
    public class ResearchResultConfiguration : IEntityTypeConfiguration<ResearchResultEntity>
    {
        public void Configure(EntityTypeBuilder<ResearchResultEntity> builder)
        {
            builder.Property(x => x.Result)
                .IsRequired()
                .HasMaxLength(128);
        }
    }
}
