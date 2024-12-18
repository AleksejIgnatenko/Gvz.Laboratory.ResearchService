﻿using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
using Gvz.Laboratory.ResearchService.Entities;
using Gvz.Laboratory.ResearchService.Exceptions;
using Gvz.Laboratory.ResearchService.Models;
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
                    UnitsOfMeasurement = product.UnitsOfMeasurement,
                    ProductName = product.ProductName,
                };

                await _context.Products.AddAsync(productEntity);
                await _context.SaveChangesAsync();
            }

            return product.Id;
        }

        public async Task<ProductModel> GetProductForResearchIdAsync(Guid researchId)
        {
            var researchEntity = await _context.Researches
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == researchId);

            if(researchEntity == null)
            {
                throw new RepositoryException("Продукт не найден");
            }

            var product = ProductModel.Create(researchEntity.Product.Id, researchEntity.Product.ProductName, researchEntity.Product.UnitsOfMeasurement);

            return product;
        }

        public async Task<ProductEntity?> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<ProductEntity?> GetProductByNameAsync(string productName)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductName.Equals(productName));
        }

        public async Task<Guid> UpdateProductAsync(ProductDto product)
        {
            await _context.Products 
                .Where(p => p.Id == product.Id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.ProductName, product.ProductName)
                    .SetProperty(p => p.UnitsOfMeasurement, product.UnitsOfMeasurement) 
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
