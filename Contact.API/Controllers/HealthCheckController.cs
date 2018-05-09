using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
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