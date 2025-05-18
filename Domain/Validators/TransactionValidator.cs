using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator()
        {
            RuleFor(transaction => transaction.AssetId)
                .NotEmpty()
                .WithMessage("AssetId cannot be empty");

            RuleFor(transaction => transaction.Quantity)
                .NotEqual(0)
                .WithMessage("Value cannot be 0");
        }
    }
}
