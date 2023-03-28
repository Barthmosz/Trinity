using Microsoft.AspNetCore.Mvc;

namespace Trinity.API.Controllers.Health
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("/v1/health")]
        public IActionResult CheckHealthAsync()
        {
            return Ok();
        }
    }
}
