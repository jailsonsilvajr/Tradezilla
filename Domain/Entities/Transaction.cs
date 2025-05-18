using Domain.Exceptions;
using Domain.Validators;

namespace Domain.Entities
{
    public class Transaction
    {
        private static readonly TransactionValidator _validator = new TransactionValidator();
        public Guid TransactionId { get; }
        public Guid AssetId { get; }
        public decimal Value { get; }
        public Asset? Asset { get; }

        public Transaction(Guid transactionId, Guid assetId, decimal value)
        {
            TransactionId = transactionId;
            AssetId = assetId;
            Value = value;
        }

        public static Transaction Create(Guid assetId, decimal value)
        {
            var newTransaction = new Transaction(Guid.NewGuid(), assetId, value);
            var validationResult = _validator.Validate(newTransaction);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create transaction", validationResult.Errors);
            }
            return newTransaction;
        }
    }
}
