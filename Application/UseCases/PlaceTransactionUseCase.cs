using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Enums;

namespace Application.UseCases
{
    public class PlaceTransactionUseCase : IPlaceTransaction
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlaceTransactionUseCase(IWalletRepository walletRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PlaceTransactionAsync(TransactionDto transactionDto)
        {
            var wallet = await _walletRepository.GetWalletByAccountIdAsync(transactionDto.AccountId);
            wallet.AddTransaction(
                transactionDto.AssetName, 
                transactionDto.Quantity, 
                (TransactionType)transactionDto.TransactionType);

            await _walletRepository.RegisterAssetAsync(wallet);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
