namespace Gvz.Laboratory.ResearchService.Contracts
{
    public record GetResearchResultsResponse(
        Guid Id,
        string ResearchName,
        int BatchNumber,
        string Result
        );
}
