using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases
{
    public class TransactionUseCase : ITransaction
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionUseCase(IAccountRepository accountRepository, IAssetRepository assetRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _assetRepository = assetRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PlaceTransactionAsync(TransactionDto transactionDto)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(transactionDto.AccountId) ?? throw new EntityNotFoundException($"Account {transactionDto.AccountId} not found");
            var asset = account.Assets.FirstOrDefault(a => a.AssetName == transactionDto.AssetName);
            asset ??= Asset.Create(transactionDto.AccountId, transactionDto.AssetName);

            var transaction = Transaction.Create(
                asset.AssetId,
                transactionDto.Value);

            asset.AddTransation(transaction);

            _assetRepository.Insert(asset);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
