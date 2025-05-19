using Domain.Entities;
using Domain.Enums;
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

            RuleFor(transaction => transaction.TransactionType)
                .IsInEnum()
                .WithMessage("TransactionType must be a valid enum value");

            RuleFor(transaction => transaction.TransactionType)
                .Must((transaction, type) 
                    => (type == TransactionType.Credit && transaction.Quantity > 0)
                    || (type == TransactionType.Debit && transaction.Quantity < 0))
                .WithMessage("Quantity is not allowed for transaction type");
        }
    }
}
