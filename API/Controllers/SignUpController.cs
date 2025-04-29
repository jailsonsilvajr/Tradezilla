using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        [HttpPost]
        public IActionResult SignUp([FromBody] SignUpDto signUpDto)
        {
            return Ok();
        }
    }
}
