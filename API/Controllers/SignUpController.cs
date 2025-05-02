using API.DTOs;
using Application.DTOs;
using Application.Ports.Driving;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SignUp(
            [FromServices] ISignUp _signUpUseCase,
            [FromBody] AccountDto accountDto)
        {
            try
            {
                var accountId = await _signUpUseCase.SingUpAsync(accountDto);
                accountDto.AccountId = accountId;

                return Ok(accountDto);
            }
            catch (ValidationException ex)
            {
                return StatusCode(422, new ErrorResponseDto
                {
                    ErrorMessages = ex.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseDto
                {
                    ErrorMessages = new List<string> { ex.Message }
                });
            }
        }
    }
}
