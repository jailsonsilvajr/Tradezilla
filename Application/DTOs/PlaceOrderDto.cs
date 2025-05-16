namespace Application.DTOs
{
    public class PlaceOrderDto
    {
        public Guid AccountId { get; set; }
        public string? MarketId { get; set; }
        public string? Side { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
