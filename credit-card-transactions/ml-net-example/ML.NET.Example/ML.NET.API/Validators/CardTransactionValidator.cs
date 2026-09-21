using FluentValidation;
using ML.NET.API.Models;

namespace ML.NET.API.Validators;

public class CardTransactionValidator : AbstractValidator<CardTransaction>
{
    public CardTransactionValidator()
    {
        RuleFor(x => x).NotNull();
        RuleFor(x => x.Card)
            .GreaterThan(0);
        RuleFor(x => x.Year)
            .GreaterThan(1990);
        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12);
        RuleFor(x => x.Day)
            .InclusiveBetween(1, 31);
    }
}