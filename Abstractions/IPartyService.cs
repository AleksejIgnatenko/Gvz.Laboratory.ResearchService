using Gvz.Laboratory.ResearchService.Dto;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IPartyService
    {
        Task<Guid> CreatePartyAsync(PartyDto partyDto);
        Task<MemoryStream> CreationOfAQualityAndSafetyCertificateAsync(Guid partyId);
    }
}