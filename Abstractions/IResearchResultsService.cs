using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IResearchResultsService
    {
        Task DeleteResearchResultsAsync(List<Guid> ids);
        Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByResearchIdForPageAsync(Guid researchId, int pageNumber);
        Task<Guid> UpdateResearchResultAsync(Guid id, string result);
    }
}