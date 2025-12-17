using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace RateLimiter.Controllers
{
    [ApiController]
    [Route("api/ratelimit")]
    [EnableRateLimiting("per-user-policy")]
    public class RateLimiterController : ControllerBase
    {
        [HttpGet("check")]
        public IActionResult CheckRequest()
        {
             
            return Ok(new
            {
                allowed = true,
                message = "Request allowed"
            });
        }
    }
}
