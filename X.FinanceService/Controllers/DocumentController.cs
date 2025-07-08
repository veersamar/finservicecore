using X.Finance.Business.DataObjects;
using X.Finance.Business.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;

namespace X.Finance.API.Controllers
{
    [Route("api/document")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IAccountDocumentService _service;

        public DocumentController(IAccountDocumentService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AccountDocumentData document)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _service.CreateAccountDocumentAsync(document);
            return Ok(new { Id = id });
        }

        [HttpGet("getDocuments")]
        public async Task<IActionResult> GetAll()
        {
            AccountDocumentData document = null;// await _service.GetAccountDocumentAsync(id);
            if (document == null)
                return NotFound();
            return Ok(document);
        }

    }    
}
