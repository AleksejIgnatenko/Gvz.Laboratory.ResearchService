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
        private readonly IResearchResultsService _researchResultsService;

        public ResearchRepository(GvzLaboratoryResearchServiceDbContext context, IProductRepository productRepository, IResearchResultsService researchResultsService)
        {
            _context = context;
            _productRepository = productRepository;
            _researchResultsService = researchResultsService;
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

        public async Task<(List<ResearchModel> researches, int numberResearches)> GetResearchesByProductIdForPageAsync(Guid productId, int pageNumber)
        {
            var researchEntities = await _context.Researches
                    .AsNoTracking()
                    .Where(r => r.Product.Id == productId)
                    .Include(r => r.Product)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();

            if (!researchEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                researchEntities = await _context.Researches
                    .Where(r => r.Product.Id == productId)
                    .Include(r => r.Product)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberResearches = await _context.Researches
                .Where(r => r.Product.Id == productId)
                .CountAsync();

            var researches = researchEntities.Select(r => ResearchModel.Create(
                r.Id,
                r.ResearchName,
                ProductModel.Create(r.Product.Id, r.Product.ProductName, r.Product.UnitsOfMeasurement),
                false).research).ToList();

            return (researches, numberResearches);
        }

        public async Task<List<ResearchModel>> GetResearchesAsync()
        {
            var researchEntities = await _context.Researches
                    .AsNoTracking()
                    .Include(r => r.Product)
                    .OrderByDescending(r => r.DateCreate)
                    .ToListAsync();

            var researches = researchEntities.Select(r => ResearchModel.Create(
                r.Id,
                r.ResearchName,
                ProductModel.Create(r.Product.Id, r.Product.ProductName, r.Product.UnitsOfMeasurement),
                false).research).ToList();

            return researches;
        }

        public async Task<(List<ResearchModel> researches, int numberResearches)> GetResearchesForPageAsync(int pageNumber)
        {
            var researchEntities = await _context.Researches
                .AsNoTracking()
                .Include(r => r.Product)
                .OrderByDescending(r => r.DateCreate)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            if (!researchEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                researchEntities = await _context.Researches
                    .AsNoTracking()
                    .Include(r => r.Product)
                    .OrderByDescending(r => r.DateCreate)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberResearches = await _context.Researches.CountAsync();

            var researches = researchEntities.Select(r => ResearchModel.Create(
                r.Id,
                r.ResearchName,
                ProductModel.Create(r.Product.Id, r.Product.ProductName, r.Product.UnitsOfMeasurement),
                false).research).ToList();

            return (researches, numberResearches);
        }


        public async Task<List<ResearchEntity>?> GetResearchEntitiesByProductIdAsync(Guid productId)
        {
            return await _context.Researches
                .Where(r => r.Product.Id == productId)
                .ToListAsync();
        }

        public async Task<ResearchEntity?> GetResearchEntitiesByIdAsync(Guid researchId)
        {
            return await _context.Researches
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == researchId);
        }

        public async Task<List<ResearchModel>?> GetResearchesByProductIdAsync(Guid productId)
        {
            var researchEntities = await _context.Researches
                .AsNoTracking()
                .Where(r => r.Product.Id == productId)
                .ToListAsync();

            var researches = researchEntities.Select(r => ResearchModel.Create(
                r.Id,
                r.ResearchName,
                ProductModel.Create(r.Product.Id, r.Product.ProductName, r.Product.UnitsOfMeasurement),
                false).research).ToList();

            return researches;
        }

        public async Task<(List<ResearchModel> researches, int numberResearches)> SearchResearchesAsync(string searchQuery, int pageNumber)
        {
            var researchEntities = await _context.Researches
                    .AsNoTracking()
                    .Include(r => r.Product)
                    .Where(r => 
                        r.ResearchName.ToLower().Contains(searchQuery.ToLower()) ||
                        r.Product.ProductName.ToLower().Contains(searchQuery.ToLower()))
                    .OrderByDescending(r => r.DateCreate)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();

            var numberResearches = await _context.Researches
                    .AsNoTracking()
                    .CountAsync(r =>
                        r.ResearchName.ToLower().Contains(searchQuery.ToLower()) ||
                        r.Product.ProductName.ToLower().Contains(searchQuery.ToLower()));

            var researches = researchEntities.Select(r => ResearchModel.Create(
                r.Id,
                r.ResearchName,
                ProductModel.Create(r.Product.Id, r.Product.ProductName, r.Product.UnitsOfMeasurement),
                false).research).ToList();

            return (researches, numberResearches);
        }


        public async Task<Guid> UpdateResearchAsync(ResearchModel research, Guid productId)
        {
            var existingResearch = await _context.Researches
                .Include(r => r.Product)
                .Include(r => r.ResearchResults)
                .FirstOrDefaultAsync(r => r.Id == research.Id)
                ?? throw new RepositoryException("Исследование не найдено");

            var existingResearchName = await _context.Researches
                .Where(r => (r.Product.Id == productId) && (r.ResearchName == research.ResearchName) && (r.ResearchName != existingResearch.ResearchName))
                .FirstOrDefaultAsync();

            if (existingResearchName == null)
            {
                if(existingResearch.Product.Id != productId)
                {
                    existingResearch.ResearchResults.Clear();
                }

                var existingProduct = await _productRepository.GetProductByIdAsync(productId)
                   ?? throw new RepositoryException("Продукт не найден");

                existingResearch.ResearchName = research.ResearchName;
                existingResearch.Product = existingProduct;

                await _context.SaveChangesAsync();

                //var researchResultId = await _researchResultsService.AddResearchResultToParties(existingResearch.Id, existingResearch.Product.Id);
            }
            else
            {
                throw new RepositoryException("У выбранного продукта уже есть исследование с таким названием.");
            }

            return research.Id;
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
