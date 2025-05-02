using API.DTOs;
using Application.Ports.Driving;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(
            [FromServices] IDeleteAccountUseCase _deleteAccountUseCase, 
            [FromQuery] Guid accountId)
        {
            try
            {
                await _deleteAccountUseCase.DeleteAccountByIdAsync(accountId);
                return Ok();
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
