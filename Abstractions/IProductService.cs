using Gvz.Laboratory.ResearchService.Models;

namespace Gvz.Laboratory.ResearchService.Abstractions
{
    public interface IProductService
    {
        Task<ProductModel> GetProductForResearchIdAsync(Guid researchId);
    }
}