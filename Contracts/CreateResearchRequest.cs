namespace Gvz.Laboratory.ResearchService.Contracts
{
    public record CreateResearchRequest(
        string ResearchName,
        Guid ProductId
        );
}
