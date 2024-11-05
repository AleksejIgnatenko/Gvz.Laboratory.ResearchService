namespace Gvz.Laboratory.ResearchService.Entities
{
    public class ResearchEntity
    {
        public Guid Id { get; set; }
        public string ResearchName { get; set; } = string.Empty;
        public ProductEntity Product { get; set; } = new ProductEntity();
        public List<ResearchResultEntity> ResearchResults { get; set; } = new List<ResearchResultEntity>();
        public DateTime DateCreate { get; set; }
    }
}
