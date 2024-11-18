namespace Gvz.Laboratory.ResearchService.Contracts
{
    public record GetResearchResultsResponseWrapper(
        List<GetResearchResultsResponse> ResearchResults,
        int NumberResearchResults
        );
}
