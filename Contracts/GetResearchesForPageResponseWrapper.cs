namespace Gvz.Laboratory.ResearchService.Contracts
{
    public record GetResearchesForPageResponseWrapper(
        List<GetResearchesForPageResponse> Researches,
        int NumberResearches
        );
}
