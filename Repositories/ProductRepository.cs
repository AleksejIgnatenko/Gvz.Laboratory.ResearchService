using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
using Gvz.Laboratory.ResearchService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ResearchService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly GvzLaboratoryResearchServiceDbContext _context;

        public ProductRepository(GvzLaboratoryResearchServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateProductAsync(ProductDto product)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductName.Equals(product.ProductName));

            if (existingProduct == null)
            {
                var productEntity = new ProductEntity
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                };

                await _context.Products.AddAsync(productEntity);
                await _context.SaveChangesAsync();
            }

            return product.Id;
        }

        public async Task<ProductEntity?> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<Guid> UpdateProductAsync(ProductDto product)
        {
            await _context.Products 
                .Where(s => s.Id == product.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.ProductName, product.ProductName)
                 );

            return product.Id;
        }

        public async Task DeleteProductsAsync(List<Guid> ids)
        {
            await _context.Products
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
