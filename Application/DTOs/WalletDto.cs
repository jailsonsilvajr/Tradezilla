namespace Application.DTOs
{
    public class WalletDto
    {
        public List<AssetDto> Assets { get; set; } = new List<AssetDto>();
        public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}
