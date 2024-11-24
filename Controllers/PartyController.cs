using Gvz.Laboratory.ResearchService.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.ResearchService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartyController : ControllerBase
    {
        private readonly IPartyService _partyService;

        public PartyController(IPartyService partyService)
        {
            _partyService = partyService;
        }

        [HttpGet]
        [Route("creationOfAQualityAndSafetyCertificateAsync")]
        //[Authorize]
        public async Task<ActionResult> CreationOfAQualityAndSafetyCertificateAsync(Guid partyId)
        {
            var stream = await _partyService.CreationOfAQualityAndSafetyCertificateAsync(partyId);
            var fileName = "ResearchCertificate.docx";

            return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }
    }
}