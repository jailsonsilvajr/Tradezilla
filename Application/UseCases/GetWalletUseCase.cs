using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;

namespace Application.UseCases
{
    public class GetWalletUseCase : IGetWallet
    {
        private readonly IWalletRepository _walletRepository;

        public GetWalletUseCase(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletDto> GetWalletAsync(Guid accountId)
        {
            var wallet = await _walletRepository.GetWalletByAccountIdAsync(accountId);
            var walletDto = new WalletDto();

            walletDto.Assets = wallet.Assets.Select(asset => new AssetDto
            {
                AssetName = asset.GetAssetName(),
                Balance = asset.GetBalance()
            }).ToList();

            walletDto.Transactions = wallet.Transactions.Select(t => new TransactionDto
            {
                AssetName = wallet.Assets.First(a => a.GetId() == t.GetAssetId()).GetAssetName(),
                Quantity = t.GetQuantity(),
                TransactionType = (int)t.GetTransactionType()
            }).ToList();

            walletDto.Orders = wallet.Orders.Select(o => new OrderDto
            {
                Market = o.GetMarket(),
                Quantity = o.GetQuantity(),
                Price = o.GetPrice(),
                Side = o.GetSide(),
                Status = o.GetStatus(),
                Date = o.GetCreatedDate()
            }).ToList();

            return walletDto;
        }
    }
}
