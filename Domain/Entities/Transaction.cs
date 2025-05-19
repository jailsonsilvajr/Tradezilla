using Domain.Enums;
using Domain.Exceptions;
using Domain.Validators;

namespace Domain.Entities
{
    public class Transaction
    {
        private static readonly TransactionValidator _validator = new TransactionValidator();
        public Guid TransactionId { get; }
        public Guid AssetId { get; }
        public decimal Quantity { get; }
        public TransactionType TransactionType { get; }
        public Asset? Asset { get; }

        public Transaction(Guid transactionId, Guid assetId, decimal quantity, TransactionType transactionType)
        {
            TransactionId = transactionId;
            AssetId = assetId;
            Quantity = quantity;
            TransactionType = transactionType;

            Validate(this);
        }

        public static Transaction Create(Guid assetId, decimal quantity, TransactionType transactionType)
        {
            var newTransaction = new Transaction(Guid.NewGuid(), assetId, quantity, transactionType);
            Validate(newTransaction);
            return newTransaction;
        }

        private static void Validate(Transaction transaction)
        {
            var validationResult = _validator.Validate(transaction);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create transaction", validationResult.Errors);
            }
        }
    }
}
