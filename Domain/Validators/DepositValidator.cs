using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class DepositValidator : AbstractValidator<Deposit>
    {
        public DepositValidator()
        {
            RuleFor(deposit => deposit.AssetId)
                .NotEmpty()
                .WithMessage("AssetId cannot be empty");

            RuleFor(deposit => deposit.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");
        }
    }
}
