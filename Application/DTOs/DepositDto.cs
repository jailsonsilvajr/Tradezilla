namespace Application.DTOs
{
    public class DepositDto
    {
        public Guid AccountId { get; set; }
        public string? AssetId { get; set; }
        public decimal Quantity { get; set; }
    }
}
