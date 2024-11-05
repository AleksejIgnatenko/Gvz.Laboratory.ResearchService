using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Contracts;
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
        public async Task<ActionResult> DeleteResearchAsync(int pageNumber)
        {
            var (researches, numberResearches) = await _researchService.GetResearchesForPageAsync(pageNumber);

            var response = researches.Select(r => new GetResearchesForPageResponse(r.Id, r.ResearchName)).ToList();

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
        public async Task<ActionResult> DeleteProduDeleteResearchAsyncctAsync([FromBody] List<Guid> ids)
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
