using API.DTOs;
using Application.DTOs;
using Application.Ports.Driving;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp(
            [FromServices] ISignUp _signUpUseCase,
            [FromBody] SignUpDto signUpDto)
        {
            try
            {
                var accountId = await _signUpUseCase.SingUpAsync(signUpDto);
                return Ok(accountId);
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

        [HttpGet("getAccount")]
        public async Task<IActionResult> GetAccount(
            [FromServices] IGetAccount _getAccount,
            [FromQuery] Guid accountId)
        {
            try
            {
                var account = await _getAccount.GetAccountByAccountIdAsync(accountId);
                return Ok(account);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(422, new ErrorResponseDto
                {
                    ErrorMessages = new List<string> { ex.Message }
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
