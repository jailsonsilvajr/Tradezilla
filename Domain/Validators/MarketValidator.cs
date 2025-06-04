using Domain.Entities;
using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators
{
    public class MarketValidator : AbstractValidator<Market>
    {
        public MarketValidator()
        {
            var marketMaxLength = 7;
            RuleFor(x => x.GetValue())
                .NotEmpty()
                .WithMessage("Market cannot be null, empty or whitespace")
                .Length(1, marketMaxLength)
                .WithMessage($"Market must be between 1 and {marketMaxLength} characters long");
        }
    }
}
