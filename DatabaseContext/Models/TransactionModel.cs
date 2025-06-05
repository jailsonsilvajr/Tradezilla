namespace DatabaseContext.Models
{
    public class TransactionModel
    {
        public Guid TransactionId { get; set; }
        public Guid AssetId { get; set; }
        public int Quantity { get; set; }
        public AssetModel? Asset { get; set; }
        public int TransactionType { get; set; }
    }
}
