namespace Application.DTOs
{
    public class DepositDto
    {
        public Guid AccountId { get; set; }
        public string? AssetName { get; set; }
        public decimal Quantity { get; set; }
    }
}
