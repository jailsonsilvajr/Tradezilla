namespace DatabaseContext.Models
{
    public class AssetModel
    {
        public Guid AssetId { get; set; }
        public Guid AccountId { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public AccountModel? Account { get; set; }
        public ICollection<TransactionModel> Transactions { get; set; } = [];
    }
}
