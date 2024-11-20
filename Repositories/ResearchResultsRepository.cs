using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
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

        public async Task CreateResearchResultsAsync(Guid partyId, Guid productId)
        {
            var partyEntity = await _partyRepository.GetPartyEntityByIdAsync(partyId);

            var productResearchEntities = await _researchRepository.GetResearchEntitiesByProductIdAsync(productId);

            if ((productResearchEntities != null) && (partyEntity != null))
            {
                var researchResults = new List<ResearchResultEntity>();

                foreach (var research in productResearchEntities)
                {
                    var researchResultEntity = new ResearchResultEntity
                    {
                        Id = Guid.NewGuid(),
                        Research = research,
                        Party = partyEntity,
                        Result = string.Empty,
                        DateCreate = DateTime.Now,
                    };

                    researchResults.Add(researchResultEntity);
                }
                await _context.ResearchResults.AddRangeAsync(researchResults);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByResearchIdForPageAsync(Guid researchId, int pageNumber)
        {
            var researchResultEntities = await _context.ResearchResults
                    .Where(r => r.Research.Id == researchId)
                    .Include(r => r.Research)
                    .Include(r => r.Party)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();

            if (!researchResultEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                researchResultEntities = await _context.ResearchResults
                    .Where(r => r.Research.Id == researchId)
                    .Include(r => r.Research)
                    .Include(r => r.Party)
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
                        r.Party.Surname,
                        r.Party.Note),
                    r.Result,
                    false).researchResult).ToList();

            return (researchResults, numberResearchResults);
        }

        public async Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByPartyIdForPageAsync(Guid partyId, int pageNumber)
        {
            var researchResultEntities = await _context.ResearchResults
                    .Where(r => r.Party.Id == partyId)
                    .Include(r => r.Research)
                    .Include(r => r.Party)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();

            if (!researchResultEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                researchResultEntities = await _context.ResearchResults
                    .Where(r => r.Party.Id == partyId)
                    .Include(r => r.Research)
                    .Include(r => r.Party)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberResearchResults = await _context.ResearchResults
                    .Where(r => r.Party.Id == partyId)
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
