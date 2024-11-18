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

        public async Task<(List<PartyModel> parties, int numberParties)> GetUserPartiesForPageAsync(Guid userId, int pageNumber)
        {
            var partyEntities = await _context.Parties
                    .AsNoTracking()
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();

            if (!partyEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                partyEntities = await _context.Parties
                    .AsNoTracking()
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberParties = await _context.Parties.CountAsync();

            var parties = partyEntities.Select(p => PartyModel.Create(
                p.Id,
                p.BatchNumber,
                p.DateOfReceipt,
                p.ProductName,
                p.SupplierName,
                p.ManufacturerName,
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                p.Surname,
                p.Note
                )).ToList();

            return (parties, numberParties);
        }

        public async Task<PartyEntity?> GetPartyEntityByIdAsync(Guid id)
        {
            return await _context.Parties.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Guid> UpdatePartyAsync(PartyDto party)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id)
                ?? throw new InvalidOperationException($"Party with Id '{party.Id}' was not found.");

            existingParty.BatchNumber = party.BatchNumber;
            existingParty.DateOfReceipt = party.DateOfReceipt;
            existingParty.ProductName = party.ProductName;
            existingParty.SupplierName = party.SupplierName;
            existingParty.ManufacturerName = party.ManufacturerName;
            existingParty.BatchSize = party.BatchSize;
            existingParty.SampleSize = party.SampleSize;
            existingParty.TTN = party.TTN;
            existingParty.DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety;
            existingParty.TestReport = party.TestReport;
            existingParty.DateOfManufacture = party.DateOfManufacture;
            existingParty.ExpirationDate = party.ExpirationDate;
            existingParty.Packaging = party.Packaging;
            existingParty.Marking = party.Marking;
            existingParty.Result = party.Result;
            existingParty.Note = party.Note;

            await _context.SaveChangesAsync();

            return party.Id;
        }

        public async Task DeletePartiesAsync(List<Guid> ids)
        {
            await _context.Parties
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
