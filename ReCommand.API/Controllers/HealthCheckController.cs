using Microsoft.AspNetCore.Mvc;

namespace ReCommand.API.Controllers
{
    [Route("HealthCheck")]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        [HttpHead]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}