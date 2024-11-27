namespace Gvz.Laboratory.ResearchService.Models
{
    public class ProductModel
    {
        public Guid Id { get;  }
        public string ProductName { get; } = string.Empty;
        public string UnitsOfMeasurement { get; } = string.Empty;
        //public List<ResearchModel> Researches { get; set; } = new List<ResearchModel>();

        public ProductModel()
        {
        }

        public ProductModel(Guid id, string productName, string unitsOfMeasurement)
        {
            Id = id;
            ProductName = productName;
            UnitsOfMeasurement = unitsOfMeasurement;
        }

        public static ProductModel Create(Guid id, string productName, string unitsOfMeasurement)
        {
            ProductModel product = new ProductModel(id, productName, unitsOfMeasurement);
            return product;
        }
    }
}
