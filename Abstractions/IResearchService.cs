﻿using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IResearchService
    {
        Task<Guid> CreateResearchAsync(Guid id, string name, Guid productId);
        Task DeleteResearchAsync(List<Guid> ids);
        Task<(List<ResearchModel> researches, int numberResearches)> GetResearchesByProductIdForPageAsync(Guid productId, int pageNumber);
        Task<(List<ResearchModel> researches, int numberResearches)> GetResearchesForPageAsync(int pageNumber);
        Task<(List<ResearchModel> researches, int numberResearches)> SearchResearchesAsync(string searchQuery, int pageNumber);
        Task<MemoryStream> ExportResearchesToExcelAsync();
        Task<Guid> UpdateResearchAsync(Guid id, string name, Guid productId);
    }
}