using Domain.Exceptions;
using Domain.Validators;

namespace Domain.Entities
{
    public class Deposit
    {
        public static readonly int MAX_ASSETID_LENGTH = 5;
        private static readonly DepositValidator _validator = new DepositValidator();
        public Guid DepositId { get; set; }
        public Guid AccountId { get; set; }
        public string AssetId { get; set; }
        public decimal Quantity { get; set; }
        public Account? Account { get; set; }

        private Deposit(Guid accountId, string assetId, decimal quantity)
        {
            AccountId = accountId;
            AssetId = assetId;
            Quantity = quantity;
        }

        public static Deposit Create(Guid accountId, string assetId, decimal quantity)
        {
            var newDeposit = new Deposit(accountId, assetId, quantity);
            var validationResult = _validator.Validate(newDeposit);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create deposit", validationResult.Errors);
            }
            return newDeposit;
        }
    }
}
