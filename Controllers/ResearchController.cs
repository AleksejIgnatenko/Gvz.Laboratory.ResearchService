using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.ResearchService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResearchController : ControllerBase
    {
        private readonly IResearchService _researchService;

        public ResearchController(IResearchService researchService)
        {
            _researchService = researchService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateResearchAsync([FromBody] CreateResearchRequest createResearchRequest)
        {
            var id = await _researchService.CreateResearchAsync(Guid.NewGuid(),
                                                        createResearchRequest.ResearchName,
                                                        createResearchRequest.ProductId);
            return Ok();
        }

        [HttpGet]
        [Route("getResearchesByProductIdForPage")]
        public async Task<ActionResult> GetResearchesByProductIdForPageAsync(Guid productId, int pageNumber)
        {
            var (researches, numberResearches) = await _researchService.GetResearchesByProductIdForPageAsync(productId, pageNumber);

            var response = researches.Select(r => new GetResearchesResponse(r.Id, r.ResearchName, r.Product.ProductName)).ToList();

            var responseWrapper = new GetResearchesForPageResponseWrapper(response, numberResearches);

            return Ok(responseWrapper);
        }

        [HttpGet]
        public async Task<ActionResult> GetResearchesForPageAsync(int pageNumber)
        {
            var (researches, numberResearches) = await _researchService.GetResearchesForPageAsync(pageNumber);

            var response = researches.Select(r => new GetResearchesResponse(r.Id, r.ResearchName, r.Product.ProductName)).ToList();

            var responseWrapper = new GetResearchesForPageResponseWrapper(response, numberResearches);

            return Ok(responseWrapper);
        }

        [HttpGet]
        [Route("exportResearchesToExcel")]
        [Authorize]
        public async Task<ActionResult> ExportResearchesToExcelAsync()
        {
            var stream = await _researchService.ExportResearchesToExcelAsync();
            var fileName = "Researches.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet]
        [Route("searchResearches")]
        [Authorize]
        public async Task<ActionResult> SearchResearchesAsync(string searchQuery, int pageNumber)
        {
            var (researches, numberResearches) = await _researchService.SearchResearchesAsync(searchQuery, pageNumber);

            var response = researches.Select(r => new GetResearchesResponse(r.Id, r.ResearchName, r.Product.ProductName)).ToList();

            var responseWrapper = new GetResearchesForPageResponseWrapper(response, numberResearches);

            return Ok(responseWrapper);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateResearchAsync(Guid id, [FromBody] UpdateResearchRequest updateResearchRequest)
        {
            await _researchService.UpdateResearchAsync(id, updateResearchRequest.ResearchName, updateResearchRequest.ProductId);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteResearchAsync([FromBody] List<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No supplier IDs provided.");
            }

            await _researchService.DeleteResearchAsync(ids);

            return Ok();
        }
    }
}
