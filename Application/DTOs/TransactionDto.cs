namespace Application.DTOs
{
    public class TransactionDto
    {
        public Guid AccountId { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int TransactionType { get; set; }
    }
}
