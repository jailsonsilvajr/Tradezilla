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
        public Asset? Asset { get; }

        public Transaction(Guid transactionId, Guid assetId, decimal quantity)
        {
            TransactionId = transactionId;
            AssetId = assetId;
            Quantity = quantity;
        }

        public static Transaction Create(Guid assetId, decimal quantity)
        {
            var newTransaction = new Transaction(Guid.NewGuid(), assetId, quantity);
            var validationResult = _validator.Validate(newTransaction);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create transaction", validationResult.Errors);
            }
            return newTransaction;
        }
    }
}
