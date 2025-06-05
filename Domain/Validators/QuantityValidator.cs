using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators
{
    public class QuantityValidator : AbstractValidator<Quantity>
    {
        public QuantityValidator()
        {
            RuleFor(q => q.GetValue())
             .GreaterThan(0)
             .WithMessage("Quantity must be greater than 0");
        }
    }
}
