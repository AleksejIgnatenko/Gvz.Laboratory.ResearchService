using Gvz.Laboratory.ResearchService.Dto;
using Gvz.Laboratory.ResearchService.Entities;
using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IPartyRepository
    {
        Task<Guid> CreatePartyAsync(PartyDto party);
        Task DeletePartiesAsync(List<Guid> ids);
        Task<PartyEntity?> GetPartyEntityByIdAsync(Guid id);
        Task<List<PartyEntity>?> GetPartyEntityByProductNameAsync(string productName);
        Task<PartyModel> GetPartiesAsync(Guid partyId);
        Task<PartyEntity> UpdatePartyAsync(PartyDto party);
    }
}