namespace Gvz.Laboratory.ResearchService.Models
{
    public class ProductModel
    {
        public Guid Id { get;  }
        public string ProductName { get; } = string.Empty;
        //public List<ResearchModel> Researches { get; set; } = new List<ResearchModel>();

        public ProductModel()
        {
        }

        public ProductModel(Guid id, string productName)
        {
            Id = id;
            ProductName = productName;
        }

        public static ProductModel Create(Guid id, string productName)
        {
            ProductModel product = new ProductModel(id, productName);
            return product;
        }
    }
}
