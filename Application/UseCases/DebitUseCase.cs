using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.UseCases
{
    public class DebitUseCase : IDebit
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DebitUseCase(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task PlaceDebitAsync(TransactionDto transactionDto)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(transactionDto.AccountId) ?? throw new EntityNotFoundException($"Account {transactionDto.AccountId} not found");
            var asset = account.Assets.FirstOrDefault(a => a.GetAssetName() == transactionDto.AssetName) ?? throw new EntityNotFoundException($"Asset {transactionDto.AssetName} not found");

            var transaction = Transaction.Create(
                asset.GetId(),
                transactionDto.Quantity,
                (TransactionType)transactionDto.TransactionType);

            asset.AddTransaction(transaction);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
