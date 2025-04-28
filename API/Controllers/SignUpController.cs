using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        [HttpPost]
        public IActionResult SignUp()
        {
            return Ok();
        }
    }
}
