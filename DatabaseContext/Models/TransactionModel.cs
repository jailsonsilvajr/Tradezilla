namespace DatabaseContext.Models
{
    public class TransactionModel
    {
        public Guid TransactionId { get; set; }
        public Guid AssetId { get; set; }
        public decimal Quantity { get; set; }
        public AssetModel? Asset { get; set; }
    }
}
