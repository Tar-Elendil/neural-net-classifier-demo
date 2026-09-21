using Microsoft.AspNetCore.Mvc;
using ML.NET.API.Models;
using ML.NET.API.Services;
using ML.NET.API.Validators;
using System.Net;

namespace ML.NET.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class Transactions(IPredictClassifications predictionModel, CardTransactionValidator validator) : ControllerBase
{
    [HttpPut]
    [Route("verify")]
    public IActionResult Verify([FromBody] CardTransaction transaction)
    {
        // Validate input
        var validationResult = validator.Validate(transaction);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(x => x.ErrorMessage)
                .Aggregate((error, acc) => string.Concat(acc, "|", error));
            return BadRequest($"Invalid transaction submitted for verification {errors}");
        }

        var isFraud = predictionModel.CheckIfFraud(transaction);

        if (isFraud)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, "Transaction rejected as possible fraud");
        }

        return Ok(); 
    }
}