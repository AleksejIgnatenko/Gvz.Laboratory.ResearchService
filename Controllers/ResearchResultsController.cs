using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Contracts;
using Gvz.Laboratory.ResearchService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.ResearchService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResearchResultsController : ControllerBase
    {
        private readonly IResearchResultsService _researchResultsService;

        public ResearchResultsController(IResearchResultsService researchResultsService)
        {
            _researchResultsService = researchResultsService;
        }

        //[HttpPost]
        //public async Task<ActionResult> CreateResearchResultsAsync([FromBody] CreateResearchRequest createResearchRequest)
        //{
        //    var id = await _researchResultsService.CreateResearchAsync(Guid.NewGuid(),
        //                                                createResearchRequest.ResearchName,
        //                                                createResearchRequest.ProductId);
        //    return Ok();
        //}

        [HttpGet]
        [Route("researchResultsByResearchIdForPage")]
        public async Task<ActionResult> GetResearchResultsByResearchIdForPageAsync(Guid researchId, int pageNumber)
        {
            var (researchResults, numberResearchResults) = await _researchResultsService.GetResearchResultsByResearchIdForPageAsync(researchId, pageNumber);

            var response = researchResults.Select(r => new GetResearchResultsResponse(r.Id, r.Research.ResearchName, r.Party.BatchNumber, r.Result)).ToList();

            var responseWrapper = new GetResearchResultsResponseWrapper(response, numberResearchResults);

            return Ok(responseWrapper);
        }

        [HttpGet]
        [Route("researchResultsByPartyIdForPage")]
        public async Task<ActionResult> GetResearchResultsByPartyIdForPageAsync(Guid partyId, int pageNumber)
        {
            var (researchResults, numberResearchResults) = await _researchResultsService.GetResearchResultsByPartyIdForPageAsync(partyId, pageNumber);

            var response = researchResults.Select(r => new GetResearchResultsResponse(r.Id, r.Research.ResearchName, r.Party.BatchNumber, r.Result)).ToList();

            var responseWrapper = new GetResearchResultsResponseWrapper(response, numberResearchResults);

            return Ok(responseWrapper);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateResearchResultAsync(Guid id, [FromBody] UpdateResearchResultRequest updateResearchResultRequest)
        {
            await _researchResultsService.UpdateResearchResultAsync(id, updateResearchResultRequest.Result);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteResearchResultsAsync([FromBody] List<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No supplier IDs provided.");
            }

            await _researchResultsService.DeleteResearchResultsAsync(ids);

            return Ok();
        }
    }
}
