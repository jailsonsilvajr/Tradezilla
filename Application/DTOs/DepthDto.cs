namespace Application.DTOs
{
    public class DepthDto
    {
        public List<BuyDepthDto> Buys { get; set; } = [];
        public List<SellDepthDto> Sells { get; set; } = [];
    }

    public class BuyDepthDto
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }

    public class SellDepthDto
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
