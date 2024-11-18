using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IResearchResultsRepository
    {
        Task<Guid> CreateResearchResultsAsync(ResearchResultModel researchResult, Guid researchId, Guid partyId);
        Task DeleteResearchResultsAsync(List<Guid> ids);
        Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByResearchIdForPageAsync(Guid researchId, int pageNumber);
        Task<Guid> UpdateResearchResultAsync(ResearchResultModel researchResult);
    }
}