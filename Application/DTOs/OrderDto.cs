namespace Application.DTOs
{
    public class OrderDto
    {
        public string Market { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
