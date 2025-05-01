using API.DTOs;
using DatabaseContext;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IValidator<SignUpDto> _signUpDtoValidator;
        private readonly TradezillaContext _context;

        public SignUpController(IValidator<SignUpDto> signUpDtoValidator, TradezillaContext context)
        {
            _signUpDtoValidator = signUpDtoValidator;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
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

            var accountDto = new AccountDto
            {
                Id = Guid.NewGuid(),
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                Document = signUpDto.Document,
                Password = signUpDto.Password
            };

            await _context.InsertAccountAsync(
                accountDto.Id, 
                accountDto.Name!, 
                accountDto.Email!, 
                accountDto.Document!, 
                accountDto.Password!);

            return Ok(accountDto);
        }
    }
}
