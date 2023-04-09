using Microsoft.AspNetCore.Mvc;

namespace Trinity.API.Controllers
{
    [ApiController]
    [Route("v1/[Controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult CheckHealthAsync()
        {
            return Ok();
        }
    }
}
