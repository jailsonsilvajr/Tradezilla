namespace Application.DTOs
{
    public class TransactionDto
    {
        public Guid AccountId { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public int TransactionType { get; set; }
    }
}
