using ML.NET.API.Models;
using ML.NET.API.Services;

namespace ML.NET.API.Tests.Services;

public class PredictionServiceTests
{
    private readonly PredictionService predictionService = new();

    [Fact]
    public void Should_Return_True_For_Is_Fraud()
    {
        // Arrange
        var transaction = new CardTransaction
        {
            User = 0,
            Card = 0,
            Amount = 287.13m,
            Year = 2015,
            Month = 11,
            Day = 15,
            Time = TimeOnly.Parse("12:15"),
            UseChip = "Online Transaction",
            MerchantName = "-8194607650924472520",
            MerchantCity = "ONLINE",
            MerchantState = double.NaN.ToString(),
            Zip = double.NaN,
            MCC = 3001
        };

        // Act
        var result = predictionService.CheckIfFraud(transaction);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_False_For_Is_Fraud()
    {
        // Arrange
        var transaction = new CardTransaction
        {
            User = 0,
            Card = 0,
            Amount = 287.13m,
            Year = 2015,
            Month = 11,
            Day = 15,
            Time = TimeOnly.Parse("12:15"),
            UseChip = "Swipe Transaction",
            MerchantName = "-8194607650924472520",
            MerchantCity = "NYC",
            MerchantState = "New York",
            Zip = double.NaN,
            MCC = 3001
        };

        // Act
        var result = predictionService.CheckIfFraud(transaction);

        // Assert
        Assert.False(result);
    }
}