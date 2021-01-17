using Isaac_AuthorizationService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Isaac_AuthorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhitelistController : ControllerBase
    {
        private readonly IWhitelistService _service;

        public WhitelistController(IWhitelistService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetWhitelists()
        {
            return Ok(_service.GetWhitelists());
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWhitelist(int id)
        {
            _service.Delete(id);
            return Ok();
        }
        
        [HttpPost("{id}")]
        public IActionResult UpdateWhitelist([FromBody] Whitelist whitelist)
        {
            _service.Update(whitelist);
            return Ok();
        }
        
        [HttpPut]
        public IActionResult CreateWhitelist([FromBody] Whitelist whitelist)
        {
            _service.Create(whitelist);
            return Ok();
        }
    }
}