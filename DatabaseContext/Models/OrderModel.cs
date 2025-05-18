namespace DatabaseContext.Models
{
    public class OrderModel
    {
        public Guid OrderId { get; set; }
        public Guid AccountId { get; set; }
        public string? Market { get; set; }
        public string? Side { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int FillQuantity { get; set; }
        public decimal FillPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; }
        public AccountModel? Account { get; set; }
    }
}
