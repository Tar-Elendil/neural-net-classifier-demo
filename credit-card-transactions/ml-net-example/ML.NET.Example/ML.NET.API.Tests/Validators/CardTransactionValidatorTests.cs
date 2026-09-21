using FluentValidation.TestHelper;
using ML.NET.API.Validators;

namespace ML.NET.API.Tests.Validators;

public class CardTransactionValidatorTests
{
    private CardTransactionValidator validator = new();

    [Fact]
    public void Should_Accept_Valid_Input()
    {
        // Arrange
        var transaction = TestData.CardTransaction();

        // Act
        var result = validator.TestValidate(transaction);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Should_Reject_Invalid_Card(long card)
    {
        // Arrange
        var transaction = TestData.CardTransaction();
        transaction.Card = card;

        // Act
        var result = validator.TestValidate(transaction);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Card);
    }

    [Fact]
    public void Should_Accept_Valid_Card()
    {
        // Arrange
        var transaction = TestData.CardTransaction();
        transaction.Card = 100;

        // Act
        var result = validator.TestValidate(transaction);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(1900, 13, 32)]
    [InlineData(-2000, -12, -16)]
    public void Should_Reject_Invalid_Date(int year, int month, int day)
    {
        // Arrange
        var transaction = TestData.CardTransaction();
        transaction.Year = year;
        transaction.Month = month;
        transaction.Day = day;

        // Act
        var result = validator.TestValidate(transaction);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Year);
        result.ShouldHaveValidationErrorFor(x => x.Month);
        result.ShouldHaveValidationErrorFor(x => x.Day);
    }
}