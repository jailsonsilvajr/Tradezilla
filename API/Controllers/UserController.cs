using API.DTOs;
using Application.DTOs;
using Application.Ports.Driving;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
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

        [HttpGet("getUser")]
        public async Task<IActionResult> GetUser(
            [FromServices] IGetUser _getUser,
            [FromQuery] Guid accountId)
        {
            try
            {
                var user = await _getUser.GetUserByAccountIdAsync(accountId);
                return Ok(user);
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
