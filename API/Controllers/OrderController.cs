using API.DTOs;
using Application.DTOs;
using Application.Ports.Driving;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrder(
            [FromServices] IPlaceOrder _placeOrder,
            [FromBody] PlaceOrderDto placeOrderDto)
        {
            try
            {
                var orderId = await _placeOrder.PlaceOrder(placeOrderDto);
                return Ok(orderId);
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
            catch (Exception ex) when (ex is EntityNotFoundException || ex is InsufficientBalanceException)
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
