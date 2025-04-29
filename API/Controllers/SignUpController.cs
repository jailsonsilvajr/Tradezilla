using API.DTOs;
using API.Validators;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly SignUpDtoValidator _signUpDtoValidator;
        public SignUpController()
        {
            _signUpDtoValidator = new SignUpDtoValidator();
        }

        [HttpPost]
        public IActionResult SignUp([FromBody] SignUpDto signUpDto)
        {
            var validationResult = _signUpDtoValidator.Validate(signUpDto);
            if (!validationResult.IsValid)
            {
                return StatusCode(422, new ErrorResponseDto
                {
                    ErrorMessages = validationResult.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList()
                });
            }

            return Ok(new AccountDto
            {
                Id = Guid.NewGuid(),
                Name = signUpDto.Name!,
                Email = signUpDto.Email,
                Document = signUpDto.Document,
                Password = signUpDto.Password
            });
        }
    }
}
