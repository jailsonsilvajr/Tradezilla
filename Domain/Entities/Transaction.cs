using Domain.Enums;
using Domain.Exceptions;
using Domain.Validators;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Transaction
    {
        private static readonly TransactionValidator _validator = new TransactionValidator();
        private readonly ID _transactionId;
        private readonly ID _assetId;
        private readonly Quantity _quantity;
        private readonly TransactionType _transactionType;

        public Asset? Asset { get; }

        public Transaction(Guid transactionId, Guid assetId, int quantity, TransactionType transactionType)
        {
            _transactionId = new ID(transactionId);
            _assetId = new ID(assetId);
            _quantity = new Quantity(quantity);
            _transactionType = transactionType;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid transaction", validationResult.Errors);
            }
        }

        public Guid GetTransactionId() => _transactionId.GetValue();
        public Guid GetAssetId() => _assetId.GetValue();
        public int GetQuantity() => _quantity.GetValue();
        public TransactionType GetTransactionType() => _transactionType;

        public static Transaction Create(Guid assetId, int quantity, TransactionType transactionType)
        {
            return new Transaction(Guid.NewGuid(), assetId, quantity, transactionType);
        }
    }
}
