using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using X.Finance.Business.DataObjects;
using X.Finance.Business.Services;

namespace X.Finance.API.Controllers
{
    [Route("api/tag")]
    [ApiController]
    public class AccountAdvanceTaggingController : ControllerBase
    {
        private readonly AccountDocumentTaggingService _advanceService;

        public AccountAdvanceTaggingController(AccountDocumentTaggingService advanceService)
        {
            _advanceService = advanceService;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> Apply([FromBody] AccountAdvanceTaggingData dto)
        {
            await _advanceService.TagToDocument(dto);
            return Ok("Advance applied successfully.");
        }
    }
}
