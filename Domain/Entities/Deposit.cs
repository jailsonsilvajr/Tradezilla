namespace Domain.Entities
{
    public class Deposit
    {
        public static readonly int MAX_ASSETID_LENGTH = 5;
        public Guid DepositId { get; set; }
        public Guid AccountId { get; set; }
        public string AssetId { get; set; }
        public decimal Quantity { get; set; }
        public Account? Account { get; set; }

        public Deposit(Guid accountId, string assetId, decimal quantity)
        {
            AccountId = accountId;
            AssetId = assetId;
            Quantity = quantity;
        }
    }
}
