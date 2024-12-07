using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Contracts;
using Gvz.Laboratory.ResearchService.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<ActionResult> GetResearchResultsByResearchIdForPageAsync(Guid researchId, int pageNumber)
        {
            var (researchResults, numberResearchResults) = await _researchResultsService.GetResearchResultsByResearchIdForPageAsync(researchId, pageNumber);

            var response = researchResults.Select(r => new GetResearchResultsResponse(r.Id, r.Research.ResearchName, r.Party.BatchNumber, r.Result)).ToList();

            var responseWrapper = new GetResearchResultsResponseWrapper(response, numberResearchResults);

            return Ok(responseWrapper);
        }

        [HttpGet]
        [Route("researchResultsByPartyIdForPage")]
        [Authorize]
        public async Task<ActionResult> GetResearchResultsByPartyIdForPageAsync(Guid partyId, int pageNumber)
        {
            var (researchResults, numberResearchResults) = await _researchResultsService.GetResearchResultsByPartyIdForPageAsync(partyId, pageNumber);

            var response = researchResults.Select(r => new GetResearchResultsResponse(r.Id, r.Research.ResearchName, r.Party.BatchNumber, r.Result)).ToList();

            var responseWrapper = new GetResearchResultsResponseWrapper(response, numberResearchResults);

            return Ok(responseWrapper);
        }

        [HttpGet]
        [Route("exportResearchResultsToExcel")]
        [Authorize]
        public async Task<ActionResult> ExportResearchResultsToExcelAsync()
        {
            var stream = await _researchResultsService.ExportResearchResultsToExcelAsync();
            var fileName = "ResearchResults.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet]
        [Route("searchResearchResults")]
        [Authorize]
        public async Task<ActionResult> SearchResearchResultsAsync(string searchQuery, int pageNumber)
        {
            var (researchResults, numberResearchResults) = await _researchResultsService.SearchResearchResultsAsync(searchQuery, pageNumber);

            var response = researchResults.Select(r => new GetResearchResultsResponse(r.Id, r.Research.ResearchName, r.Party.BatchNumber, r.Result)).ToList();

            var responseWrapper = new GetResearchResultsResponseWrapper(response, numberResearchResults);

            return Ok(responseWrapper);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Manager,Worker")]
        public async Task<ActionResult> UpdateResearchResultAsync(Guid id, [FromBody] UpdateResearchResultRequest updateResearchResultRequest)
        {
            await _researchResultsService.UpdateResearchResultAsync(id, updateResearchResultRequest.Result);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Manager,Worker")]
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
