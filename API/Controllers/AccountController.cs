using API.DTOs;
using Application.Ports.Driving;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
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
