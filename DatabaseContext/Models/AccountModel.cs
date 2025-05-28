namespace DatabaseContext.Models
{
    public class AccountModel
    {
        public Guid AccountId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string Document { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ICollection<AssetModel> Assets { get; set; } = [];
        public ICollection<OrderModel> Orders { get; set; } = [];
    }
}
