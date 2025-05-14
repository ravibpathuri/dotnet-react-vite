using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReferBuddy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult Get()
        {
            return Ok("Welcome to ReferBuddy API");
        }
    }
}
