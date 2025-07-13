using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using X.Finance.Business.DataObjects;
using X.Finance.Business.Services;

namespace X.Finance.API.Controllers
{
    [Route("api/document")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IAccountDocumentService _accountDocumentService;

        public DocumentController(IAccountDocumentService accountDocumentService)
        {
            _accountDocumentService = accountDocumentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AccountDocumentData documentData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Use the service instead of direct repository access
            var id = await _accountDocumentService.CreateAccountDocumentAsync(documentData);
            return Ok(new { Id = id });
        }

        [HttpGet("getDocuments")]
        public async Task<IActionResult> GetAll()
        {
            AccountDocumentData document = null; // TODO: Implement GetAllDocuments in service
            if (document == null)
                return NotFound();
            return Ok(document);
        }
    }
}
