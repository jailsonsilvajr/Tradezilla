using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.UseCases
{
    public class CreditUseCase : ICredit
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreditUseCase(IAccountRepository accountRepository, IAssetRepository assetRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _assetRepository = assetRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PlaceCreditAsync(TransactionDto transactionDto)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(transactionDto.AccountId) ?? throw new EntityNotFoundException($"Account {transactionDto.AccountId} not found");
            var asset = account.Assets.FirstOrDefault(a => a.GetAssetName() == transactionDto.AssetName);
            asset ??= Asset.Create(transactionDto.AccountId, transactionDto.AssetName);


            var transaction = Transaction.Create(
                asset.GetId(),
                transactionDto.Quantity,
                (TransactionType)transactionDto.TransactionType);

            asset.AddTransaction(transaction);

            _assetRepository.Insert(asset);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
