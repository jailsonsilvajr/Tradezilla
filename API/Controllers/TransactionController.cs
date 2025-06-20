﻿using API.DTOs;
using Application.DTOs;
using Application.Ports.Driving;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpPost("placeCredit")]
        public async Task<IActionResult> PlaceCredit(
            [FromServices] IPlaceTransaction _placeTransaction,
            [FromBody] TransactionDto transactionDto)
        {
            try
            {
                await _placeTransaction.PlaceTransactionAsync(transactionDto);
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

        [HttpPost("placeDebit")]
        public async Task<IActionResult> PlaceDebit(
            [FromServices] IPlaceTransaction _placeTransaction,
            [FromBody] TransactionDto transactionDto)
        {
            try
            {
                await _placeTransaction.PlaceTransactionAsync(transactionDto);
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
