using Gvz.Laboratory.ResearchService.Entities;
using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IResearchResultsRepository
    {
        Task CreateResearchResultsAsync(Guid partyId, Guid productId);
        Task<Guid> AddResearchResultToPartiesAsync(ResearchEntity researchEntity);
        Task DeleteResearchResultsAsync(List<Guid> ids);
        Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByResearchIdForPageAsync(Guid researchId, int pageNumber);
        Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByPartyIdForPageAsync(Guid partyId, int pageNumber);
        Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> SearchResearchResultsAsync(string searchQuery, int pageNumber);
        Task<List<ResearchResultModel>> GetResearchResultsAsync();
        Task<Guid> UpdateResearchResultAsync(ResearchResultModel researchResult);
    }
}