using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Entities;
using Gvz.Laboratory.ResearchService.Exceptions;
using Gvz.Laboratory.ResearchService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ResearchService.Repositories
{
    public class ResearchRepository : IResearchRepository
    {
        private readonly GvzLaboratoryResearchServiceDbContext _context;
        private readonly IProductRepository _productRepository;

        public ResearchRepository(GvzLaboratoryResearchServiceDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

        public async Task<Guid> CreateResearchAsync(ResearchModel research, Guid productId)
        {
            var existingResearch = await _context.Researches.FirstOrDefaultAsync(r => r.ResearchName.Equals(research.ResearchName));

            if (existingResearch != null) { throw new RepositoryException("Такое исследование уже существует"); }

            var existingProduct = await _productRepository.GetProductByIdAsync(productId)
                ?? throw new RepositoryException("Продукт не найден");

            var researchEntity = new ResearchEntity
            {
                Id = research.Id,
                ResearchName = research.ResearchName,
                Product = existingProduct,
                DateCreate = DateTime.UtcNow,
            };

            await _context.Researches.AddAsync(researchEntity);
            await _context.SaveChangesAsync();

            return researchEntity.Id;
        }

        public async Task<(List<ResearchModel> researches, int numberResearches)> GetResearchesForPageAsync(int pageNumber)
        {
            var researchEntities = await _context.Researches
                .AsNoTracking()
                .OrderByDescending(r => r.DateCreate)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            if (!researchEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                researchEntities = await _context.Researches
                    .AsNoTracking()
                    .OrderByDescending(r => r.DateCreate)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberResearches = await _context.Researches.CountAsync();

            var researches = researchEntities.Select(r => ResearchModel.Create(
                r.Id,
                r.ResearchName,
                false).research).ToList();

            return (researches, numberResearches);
        }

        public async Task<Guid> UpdateResearchAsync(ResearchModel research, Guid productId)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(productId)
               ?? throw new RepositoryException("Продукт не найден");

            var researchEntity = await _context.Researches
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == research.Id)
                ?? throw new RepositoryException("Исследование не найдено");

            researchEntity.ResearchName = research.ResearchName;
            researchEntity.Product = existingProduct;

            await _context.SaveChangesAsync();

            return productId;
        }

        public async Task DeleteResearchesAsync(List<Guid> ids)
        {
            var researchEntities = await _context.Researches
                .Include(r => r.ResearchResults)
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();

            foreach (var researchEntity in researchEntities)
            {
                researchEntity.ResearchResults.Clear();
            }

            await _context.SaveChangesAsync();

            await _context.Researches
                .Where(r => ids.Contains(r.Id))
                .ExecuteDeleteAsync();
        }
    }
}
