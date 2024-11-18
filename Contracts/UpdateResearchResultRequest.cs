namespace Gvz.Laboratory.ResearchService.Contracts
{
    public record UpdateResearchResultRequest(
        Guid Id,
        string Result
        );
}
