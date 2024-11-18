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
        Task<(List<PartyModel> parties, int numberParties)> GetUserPartiesForPageAsync(Guid userId, int pageNumber);
        Task<Guid> UpdatePartyAsync(PartyDto party);
    }
}