using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IResearchResultsRepository
    {
        Task CreateResearchResultsAsync(Guid partyId, Guid productId);
        Task DeleteResearchResultsAsync(List<Guid> ids);
        Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByResearchIdForPageAsync(Guid researchId, int pageNumber);
        Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByPartyIdForPageAsync(Guid partyId, int pageNumber);
        Task<Guid> UpdateResearchResultAsync(ResearchResultModel researchResult);
    }
}