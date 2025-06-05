using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators
{
    public class PriceValidator : AbstractValidator<Price>
    {
        public PriceValidator()
        {
            RuleFor(price => price.GetValue())
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");
        }
    }
}
