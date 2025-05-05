using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class DepositValidator : AbstractValidator<Deposit>
    {
        public DepositValidator()
        {
            RuleFor(deposit => deposit.AccountId)
                .NotEmpty()
                .WithMessage("AccountId cannot be empty");

            RuleFor(deposit => deposit.AssetId)
                .NotEmpty()
                .WithMessage("AssetId cannot be empty")
                .Length(1, Deposit.MAX_ASSETID_LENGTH)
                .WithMessage($"AssetId must be between 1 and {Deposit.MAX_ASSETID_LENGTH} characters long");

            RuleFor(deposit => deposit.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");
        }
    }
}
