using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IResearchRepository
    {
        Task<Guid> CreateResearchAsync(ResearchModel research, Guid productId);
        Task DeleteResearchesAsync(List<Guid> ids);
        Task<(List<ResearchModel> researches, int numberResearches)> GetResearchesByProductIdForPageAsync(Guid productId, int pageNumber);
        Task<(List<ResearchModel> researches, int numberResearches)> GetResearchesForPageAsync(int pageNumber);
        Task<Guid> UpdateResearchAsync(ResearchModel research, Guid productId);
    }
}