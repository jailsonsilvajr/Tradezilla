using API.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IValidator<SignUpDto> _signUpDtoValidator;

        public SignUpController(IValidator<SignUpDto> signUpDtoValidator)
        {
            _signUpDtoValidator = signUpDtoValidator;
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
                Email = signUpDto.Email!,
                Document = signUpDto.Document!,
                Password = signUpDto.Password!
            });
        }
    }
}
