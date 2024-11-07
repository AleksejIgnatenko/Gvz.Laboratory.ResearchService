namespace Gvz.Laboratory.ResearchService.Contracts
{
    public record GetResearchesForPageResponseWrapper(
        List<GetResearchesResponse> Researches,
        int NumberResearches
        );
}
