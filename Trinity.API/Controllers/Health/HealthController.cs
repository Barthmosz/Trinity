using Microsoft.AspNetCore.Mvc;

namespace Trinity.API.Controllers.Health
{
  [ApiController]
  [Route("api/[Controller]")]
  public class HealthController : ControllerBase
  {
    [HttpGet]
    public IActionResult CheckHealth()
    {
      return Ok();
    }
  }
}
