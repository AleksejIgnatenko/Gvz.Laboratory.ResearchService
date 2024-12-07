using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Contracts;
using Gvz.Laboratory.ResearchService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.ResearchService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("getProductForResearchId")]
        [Authorize]
        public async Task<ActionResult> GetProductForResearchIdAsync(Guid researchId)
        {
            var product = await _productService.GetProductForResearchIdAsync(researchId);

            var response = new GetProductResponse(product.Id, product.ProductName, product.UnitsOfMeasurement);

            return Ok(response);
        }
    }
}
