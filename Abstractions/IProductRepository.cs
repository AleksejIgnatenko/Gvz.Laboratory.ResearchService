using Gvz.Laboratory.ResearchService.Dto;
using Gvz.Laboratory.ResearchService.Entities;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IProductRepository
    {
        Task<Guid> CreateProductAsync(ProductDto product);
        Task<ProductEntity?> GetProductByIdAsync(Guid productId);
        Task<Guid> UpdateProductAsync(ProductDto product);
        Task DeleteProductsAsync(List<Guid> ids);
    }
}