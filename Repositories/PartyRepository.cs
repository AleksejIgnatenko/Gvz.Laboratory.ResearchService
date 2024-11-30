using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;
using Gvz.Laboratory.ResearchService.Entities;
using Gvz.Laboratory.ResearchService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ResearchService.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly GvzLaboratoryResearchServiceDbContext _context;

        public PartyRepository(GvzLaboratoryResearchServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreatePartyAsync(PartyDto party)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id);
            if (existingParty == null)
            {
                var partyEntity = new PartyEntity
                {
                    Id = party.Id,
                    BatchNumber = party.BatchNumber,
                    DateOfReceipt = party.DateOfReceipt,
                    ProductName = party.ProductName,
                    SupplierName = party.SupplierName,
                    ManufacturerName = party.ManufacturerName,
                    BatchSize = party.BatchSize,
                    SampleSize = party.SampleSize,
                    TTN = party.TTN,
                    DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety,
                    TestReport = party.TestReport,
                    DateOfManufacture = party.DateOfManufacture,
                    ExpirationDate = party.ExpirationDate,
                    Packaging = party.Packaging,
                    Marking = party.Marking,
                    Result = party.Result,
                    Surname = party.Surname,
                    Note = party.Note,
                };

                await _context.Parties.AddAsync(partyEntity);
                await _context.SaveChangesAsync();
            }

            return party.Id;
        }

        public async Task<PartyEntity?> GetPartyEntityByIdAsync(Guid id)
        {
            return await _context.Parties.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PartyEntity>?> GetPartyEntityByProductNameAsync(string productName)
        {
            return await _context.Parties.Where(p => p.ProductName == productName)
                                         .Include(p => p.ResearchResults)
                                         .ToListAsync();
        }

        public async Task<PartyModel> GetPartiesAsync(Guid partyId)
        {
            var partyEntities = await _context.Parties
                    .AsNoTracking()
                    .Include(p => p.ResearchResults.OrderBy(r => r.Research.ResearchName))
                        .ThenInclude(r => r.Research)
                    .FirstOrDefaultAsync(p => p.Id == partyId);


            var parties = PartyModel.Create(
                partyEntities.Id,
                partyEntities.BatchNumber,
                partyEntities.DateOfManufacture,
                partyEntities.ProductName,
                partyEntities.SupplierName,
                partyEntities.ManufacturerName,
                partyEntities.BatchSize,
                partyEntities.SampleSize,
                partyEntities.TTN,
                partyEntities.DocumentOnQualityAndSafety,
                partyEntities.TestReport,
                partyEntities.DateOfManufacture,
                partyEntities.ExpirationDate,
                partyEntities.Packaging,
                partyEntities.Marking,
                partyEntities.Result,
                partyEntities.Surname,
                partyEntities.Note,
                partyEntities.ResearchResults.Select(r => ResearchResultModel.Create(r.Id, ResearchModel.Create(r.Research.Id, r.Research.ResearchName, false).research, r.Result, false).researchResult).ToList());

            return parties;
        }

        public async Task<PartyEntity> UpdatePartyAsync(PartyDto partyDto)
        {
            var existingParty = await _context.Parties
                .Include(p => p.ResearchResults)
                .FirstOrDefaultAsync(p => p.Id == partyDto.Id)
                ?? throw new InvalidOperationException($"Party with Id '{partyDto.Id}' was not found.");

            var partyEntity = await _context.Parties
                .AsNoTracking()
                .Include(p => p.ResearchResults)
                .FirstOrDefaultAsync(p => p.Id == partyDto.Id)
                ?? throw new InvalidOperationException($"Party with Id '{partyDto.Id}' was not found.");

            if (!existingParty.ProductName.Equals(partyDto.ProductName))
            {
                existingParty.ResearchResults.Clear();
            }

            existingParty.BatchNumber = partyDto.BatchNumber;
            existingParty.DateOfReceipt = partyDto.DateOfReceipt;
            existingParty.ProductName = partyDto.ProductName;
            existingParty.SupplierName = partyDto.SupplierName;
            existingParty.ManufacturerName = partyDto.ManufacturerName;
            existingParty.BatchSize = partyDto.BatchSize;
            existingParty.SampleSize = partyDto.SampleSize;
            existingParty.TTN = partyDto.TTN;
            existingParty.DocumentOnQualityAndSafety = partyDto.DocumentOnQualityAndSafety;
            existingParty.TestReport = partyDto.TestReport;
            existingParty.DateOfManufacture = partyDto.DateOfManufacture;
            existingParty.ExpirationDate = partyDto.ExpirationDate;
            existingParty.Packaging = partyDto.Packaging;
            existingParty.Marking = partyDto.Marking;
            existingParty.Result = partyDto.Result;
            existingParty.Note = partyDto.Note;

            await _context.SaveChangesAsync();

            return partyEntity;
        }

        public async Task DeletePartiesAsync(List<Guid> ids)
        {
            await _context.Parties
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
