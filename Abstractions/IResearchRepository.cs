using Gvz.Laboratory.ResearchService.Entities;
using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IResearchRepository
    {
        Task<Guid> CreateResearchAsync(ResearchModel research, Guid productId);
        Task DeleteResearchesAsync(List<Guid> ids);
        Task<(List<ResearchModel> researches, int numberResearches)> GetResearchesByProductIdForPageAsync(Guid productId, int pageNumber);
        Task<(List<ResearchModel> researches, int numberResearches)> GetResearchesForPageAsync(int pageNumber);
        Task<List<ResearchEntity>?> GetResearchEntitiesByProductIdAsync(Guid productId);
        Task<List<ResearchModel>?> GetResearchesByProductIdAsync(Guid productId);
        Task<List<ResearchModel>> GetResearchesAsync();
        Task<ResearchEntity?> GetResearchEntitiesByIdAsync(Guid researchId);
        Task<(List<ResearchModel> researches, int numberResearches)> SearchResearchesAsync(string searchQuery, int pageNumber);
        Task<Guid> UpdateResearchAsync(ResearchModel research, Guid productId);
    }
}