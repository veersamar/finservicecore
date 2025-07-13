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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Apply([FromBody] AccountAdvanceTaggingData dto)
        {
            if (dto == null)
            {
                return BadRequest("Request data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _advanceService.TagToDocument(dto);
                //_logger.LogInformation("Advance tagging successfully applied for DocId: {DocId}, RefDocId: {RefDocId}", dto.DocId, dto.RefDocID);
                return Ok("Advance applied successfully.");
            }
            catch (ArgumentException ex)
            {
                //_logger.LogWarning(ex, "Invalid arguments for advance tagging: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error applying advance tagging for DocId: {DocId}, RefDocId: {RefDocId}", dto.DocId, dto.RefDocID);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the advance tagging.");
            }
        }
    }
}
