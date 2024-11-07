namespace Gvz.Laboratory.ResearchService.Contracts
{
    public record GetResearchesResponse(
        Guid Id,
        string ResearchName,
        string ProductName
        );
}
