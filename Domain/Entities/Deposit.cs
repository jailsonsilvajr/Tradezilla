using Domain.Exceptions;
using Domain.Validators;

namespace Domain.Entities
{
    public class Deposit
    {
        private static readonly DepositValidator _validator = new DepositValidator();
        public Guid DepositId { get; }
        public Guid AssetId { get; }
        public decimal Quantity { get; }
        public Asset? Asset { get; }

        private Deposit(Guid depositId, Guid assetId, decimal quantity)
        {
            DepositId = depositId;
            AssetId = assetId;
            Quantity = quantity;
        }

        public static Deposit Create(Guid assetId, decimal quantity)
        {
            var newDeposit = new Deposit(Guid.NewGuid(), assetId, quantity);
            var validationResult = _validator.Validate(newDeposit);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create deposit", validationResult.Errors);
            }
            return newDeposit;
        }
    }
}
