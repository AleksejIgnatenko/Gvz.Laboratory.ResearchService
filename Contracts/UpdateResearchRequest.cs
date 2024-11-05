namespace Gvz.Laboratory.ResearchService.Contracts
{
    public record UpdateResearchRequest(
        Guid Id,
        string ResearchName,
        Guid ProductId
        );
}
