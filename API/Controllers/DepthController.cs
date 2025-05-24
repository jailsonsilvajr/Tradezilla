using API.DTOs;
using Application.Ports.Driving;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/depths")]
    [ApiController]
    public class DepthController : ControllerBase
    {
        [HttpGet("getDepth")]
        public async Task<IActionResult> GetDepth(
            [FromQuery] string marketId,
            [FromQuery] int precision,
            [FromServices] IGetDepth getDepth)
        {
            try
            {
                var result = await getDepth.ExecuteAsync(marketId, precision);
                return Ok(result);
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
