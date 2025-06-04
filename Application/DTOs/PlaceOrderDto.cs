namespace Application.DTOs
{
    public class PlaceOrderDto
    {
        public Guid AccountId { get; set; }
        public string MarketId { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
