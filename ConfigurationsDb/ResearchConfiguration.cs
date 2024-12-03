using Gvz.Laboratory.ResearchService.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ResearchService.ConfigurationsDb
{
    public class ResearchConfiguration : IEntityTypeConfiguration<ResearchEntity>
    {
        public void Configure(EntityTypeBuilder<ResearchEntity> builder)
        {
            builder.Property(x => x.ResearchName)
                .IsRequired()
                .HasMaxLength(128);
        }
    }
}
