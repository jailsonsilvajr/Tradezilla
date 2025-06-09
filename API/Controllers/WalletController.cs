using API.DTOs;
using Application.Ports.Driving;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        [HttpGet("getWallet")]
        public async Task<IActionResult> GetWallet(
            [FromServices] IGetWallet _getWalletUseCase,
            [FromQuery] Guid accountId)
        {
            try
            {
                var walletDto = await _getWalletUseCase.GetWalletAsync(accountId);
                return Ok(walletDto);
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
