using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Dto;

namespace Gvz.Laboratory.ResearchService.Services
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _partyRepository;
        private readonly IResearchRepository _researchRepository;
        private readonly IResearchResultsRepository _researchResultsRepository;

        public PartyService(IPartyRepository partyRepository, IResearchRepository researchRepository, IResearchResultsRepository researchResultsRepository)
        {
            _partyRepository = partyRepository;
            _researchRepository = researchRepository;
            _researchResultsRepository = researchResultsRepository;
        }

        public async Task<Guid> CreatePartyAsync(PartyDto partyDto)
        {
            var addPartyDtoId = await _partyRepository.CreatePartyAsync(partyDto);

            await _researchResultsRepository.CreateResearchResultsAsync(partyDto.Id, partyDto.ProductId);

            return partyDto.Id;
        }
    }
}
