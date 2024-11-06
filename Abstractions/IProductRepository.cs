using Gvz.Laboratory.ResearchService.Dto;
using Gvz.Laboratory.ResearchService.Entities;
using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IProductRepository
    {
        Task<Guid> CreateProductAsync(ProductDto product);
        Task<ProductModel> GetProductForResearchIdAsync(Guid researchId);
        Task<ProductEntity?> GetProductByIdAsync(Guid productId);
        Task<Guid> UpdateProductAsync(ProductDto product);
        Task DeleteProductsAsync(List<Guid> ids);
    }
}