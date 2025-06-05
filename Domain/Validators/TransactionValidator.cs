using Domain.Entities;
using Domain.Enums;
using FluentValidation;

namespace Domain.Validators
{
    public class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator()
        {
            RuleFor(transaction => transaction.GetTransactionType())
                .IsInEnum()
                .WithMessage("TransactionType must be a valid enum value");

            RuleFor(transaction => transaction.GetTransactionType())
                .Must((transaction, type) 
                    => (type == TransactionType.Credit && transaction.GetQuantity() > 0)
                    || (type == TransactionType.Debit && transaction.GetQuantity() < 0))
                .WithMessage("Quantity is not allowed for transaction type");
        }
    }
}
