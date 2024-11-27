namespace Gvz.Laboratory.ResearchService.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string UnitsOfMeasurement { get; set; } = string.Empty;
        public List<ResearchEntity> Researches { get; set; } = new List<ResearchEntity>();
    }
}
