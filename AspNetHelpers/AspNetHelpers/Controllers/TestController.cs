using Microsoft.AspNetCore.Mvc;

namespace AspNetHelpers.Controllers
{
    [ApiController]
    [Route(nameof(AspNetHelpers)+"/[controller]/[action]")]
    public class TestController(ILogger<TestController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("zys yz eh Test()");
        }
    }
}
