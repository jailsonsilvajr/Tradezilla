using API.DTOs;
using Application.DTOs;
using Application.Ports.Driving;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/deposit")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Deposit(
            [FromServices] IDeposit _depositUseCase,
            [FromBody] DepositDto depositDto)
        {
            try
            {
                await _depositUseCase.DepositAsync(depositDto);
                return Ok();
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
