﻿using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IResearchResultsService
    {
        Task<Guid> AddResearchResultToParties(Guid researchId, Guid productId);
        Task DeleteResearchResultsAsync(List<Guid> ids);
        Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByResearchIdForPageAsync(Guid researchId, int pageNumber);
        Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> GetResearchResultsByPartyIdForPageAsync(Guid partyId, int pageNumber);
        Task<(List<ResearchResultModel> researchResults, int numberResearchResults)> SearchResearchResultsAsync(string searchQuery, int pageNumber);
        Task<MemoryStream> ExportResearchResultsToExcelAsync();
        Task<Guid> UpdateResearchResultAsync(Guid id, string result);
    }
}