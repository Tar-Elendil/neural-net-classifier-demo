using Microsoft.AspNetCore.Mvc;
using ML.NET.API.Controllers;
using ML.NET.API.Services;
using ML.NET.API.Validators;
using Moq;

namespace ML.NET.API.Tests.Controllers;

public class TransactionsControllerTests
{
    private readonly CardTransactionValidator validator = new();
    private readonly Mock<IPredictClassifications> mockPredictor = new();

    [Fact]
    public void Verify_Returns_Bad_Request_When_Bad_Input_Received()
    {
        // Arrange
        var sut = new Transactions(mockPredictor.Object, validator);
        var transaction = TestData.CardTransaction();
        transaction.Year = -1;

        // Act
        var response = sut.Verify(transaction);

        // Assert
        var result = Assert.IsType<BadRequestObjectResult>(response);
        Assert.Contains("Invalid transaction submitted for verification", result.Value?.ToString());
    }

    [Fact]
    public void Verify_Returns_Forbidden_When_Fraud_Predicted()
    {
        // Arrange
        var sut = new Transactions(mockPredictor.Object, validator);
        var transaction = TestData.CardTransaction();
        mockPredictor.Setup(x => x.CheckIfFraud(transaction)).Returns(true);

        // Act
        var response = sut.Verify(transaction);

        // Assert
        var result = Assert.IsType<ObjectResult>(response);
        Assert.Equal(403, result.StatusCode);
        Assert.Contains("Transaction rejected as possible fraud", result.Value?.ToString());
    }

    [Fact]
    public void Verify_Returns_Ok_When_No_Fraud_Predicted()
    {
        // Arrange
        var sut = new Transactions(mockPredictor.Object, validator);
        var transaction = TestData.CardTransaction();
        mockPredictor.Setup(x => x.CheckIfFraud(transaction)).Returns(false);

        // Act
        var response = sut.Verify(transaction);

        // Assert
        var result = Assert.IsType<OkResult>(response);
    }
}