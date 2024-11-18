using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Entities;
using Gvz.Laboratory.ResearchService.Exceptions;
using Gvz.Laboratory.ResearchService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ResearchService.Repositories
{
    public class ResearchResultsRepository : IResearchResultsRepository
    {
        private readonly GvzLaboratoryResearchServiceDbContext _context;
        private readonly IResearchRepository _researchRepository;
        private readonly IPartyRepository _partyRepository;

        public ResearchResultsRepository(GvzLaboratoryResearchServiceDbContext context, IResearchRepository researchRepository, IPartyRepository partyRepository)
        {
            _context = context;
            _researchRepository = researchRepository;
            _partyRepository = partyRepository;
        }

        public async Task<Guid> CreateResearchResultsAsync(ResearchResultModel researchResult, Guid researchId, Guid partyId)
        {
            //var existingResearchResult = await _context.ResearchResults.FirstOrDefaultAsync(p => p.Research.Id == researchId);
            //if (existingResearchResult != null) { throw new RepositoryException("У партии уже есть результатна это исследование."); }

            var existingResearch = await _researchRepository.GetResearchEntityByIdAsync(researchId)
                ?? throw new RepositoryException("Исследование не найдено.");

            var existingParty = await _partyRepository.GetPartyEntityByIdAsync(researchId)
                ?? throw new RepositoryException("Исследование не найдено.");

            var researchResultEntity = new ResearchResultEntity
            {
                Id = researchResult.Id,
                Research = existingResearch,
                Party = existingParty,
                Result = researchResult.Result,
                DateCreate = DateTime.UtcNow,
            };

            await _context.ResearchResults.AddAsync(researchResultEntity);
            await _context.SaveChangesAsync();

            return researchResultEntity.Id;
        }

        public async Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByResearchIdForPageAsync(Guid researchId, int pageNumber)
        {
            var researchResultEntities = await _context.ResearchResults
                    .Where(r => r.Research.Id == researchId)
                    .Include(r => r.Research)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();

            if (!researchResultEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                researchResultEntities = await _context.ResearchResults
                    .Where(r => r.Research.Id == researchId)
                    .Include(r => r.Research)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberResearchResults = await _context.ResearchResults
                    .Where(r => r.Research.Id == researchId)
                    .CountAsync();

            var researchResults = researchResultEntities.Select(r => ResearchResultModel.Create(
                    r.Id,
                    ResearchModel.Create(r.Research.Id, r.Research.ResearchName, false).research,
                    PartyModel.Create(
                        r.Party.Id,
                        r.Party.BatchNumber,
                        r.Party.DateOfReceipt,
                        r.Party.ProductName,
                        r.Party.SupplierName,
                        r.Party.ManufacturerName,
                        r.Party.BatchSize,
                        r.Party.SampleSize,
                        r.Party.TTN,
                        r.Party.DocumentOnQualityAndSafety,
                        r.Party.TestReport,
                        r.Party.DateOfManufacture,
                        r.Party.ExpirationDate,
                        r.Party.Packaging,
                        r.Party.Marking,
                        r.Party.Result,
                        r.Party.Surname,
                        r.Party.Note),
                    r.Result,
                    false).researchResult).ToList();

            return (researchResults, numberResearchResults);
        }

        public async Task<Guid> UpdateResearchResultAsync(ResearchResultModel researchResult)
        {
            var researchResultEntity = await _context.ResearchResults
                    .FirstOrDefaultAsync(r => r.Id == researchResult.Id)
                    ?? throw new RepositoryException("Результат исследования не найден");

            researchResultEntity.Result = researchResult.Result;

            await _context.SaveChangesAsync();

            return researchResultEntity.Id;
        }

        public async Task DeleteResearchResultsAsync(List<Guid> ids)
        {
            await _context.ResearchResults
                .Where(r => ids.Contains(r.Id))
                .ExecuteDeleteAsync();
        }
    }
}
