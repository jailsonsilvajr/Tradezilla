using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        [HttpPost]
        public IActionResult SignUp([FromBody] SignUpDto signUpDto)
        {
            return Ok(new AccountDto
            {
                Id = Guid.NewGuid(),
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                Document = signUpDto.Document,
                Password = signUpDto.Password
            });
        }
    }
}
