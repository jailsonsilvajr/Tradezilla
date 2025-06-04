using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators
{
    public class BalanceValidator : AbstractValidator<Balance>
    {
        public BalanceValidator()
        {
            RuleFor(balance => balance.GetValue())
                .GreaterThanOrEqualTo(0)
                .WithMessage("Balance must be greater than or equal to 0");
        }
    }
}
