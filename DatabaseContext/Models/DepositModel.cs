namespace DatabaseContext.Models
{
    public class DepositModel
    {
        public Guid DepositId { get; set; }
        public Guid AssetId { get; set; }
        public decimal Quantity { get; set; }
        public AssetModel? Asset { get; set; }
    }
}
