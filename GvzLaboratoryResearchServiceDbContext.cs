using Gvz.Laboratory.ResearchService.ConfigurationsDb;
using Gvz.Laboratory.ResearchService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ResearchService
{
    public class GvzLaboratoryResearchServiceDbContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<PartyEntity> Parties { get; set; }
        public DbSet<ResearchEntity> Researches { get; set; }
        public DbSet<ResearchResultEntity> ResearchResults { get; set; }

        public GvzLaboratoryResearchServiceDbContext(DbContextOptions<GvzLaboratoryResearchServiceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ResearchConfiguration());
            modelBuilder.ApplyConfiguration(new ResearchResultConfiguration());
        }   
    }
}
