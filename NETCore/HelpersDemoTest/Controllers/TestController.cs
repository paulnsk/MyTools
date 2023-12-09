using System.Text.Json;
using AspNetHelpers.Middleware.LogUrl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HelpersDemoTest.Controllers
{
    [ApiController]
    [Route(nameof(AspNetHelpers)+"/[controller]/[action]")]
    public class TestController(ILogger<TestController> logger, IOptions<LogUrlMiddlewareConfig> logUrlConfigOptions) : ControllerBase
    {
        private LogUrlMiddlewareConfig? LogUrlConfig { get; } = logUrlConfigOptions?.Value ?? default;

        [HttpGet]
        public IActionResult ShowLogurlMiddlewareConfig()
        {
            return Ok($"Here is your config: {Environment.NewLine}{Environment.NewLine}{JsonSerializer.Serialize(LogUrlConfig, new JsonSerializerOptions { WriteIndented = true })}");
        }

        [HttpGet]
        public IActionResult FireException()
        {
            //todo statuscode
            throw new Exception("что бы такого сделать плохого");
        }
    }
}
