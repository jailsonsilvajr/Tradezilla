namespace DatabaseContext.Models
{
    public class AssetModel
    {
        public Guid AssetId { get; set; }
        public Guid AccountId { get; set; }
        public string? AssetName { get; set; }
        public decimal Balance { get; set; }
        public AccountModel? Account { get; set; }
        public ICollection<DepositModel> Deposits { get; set; } = [];
    }
}
