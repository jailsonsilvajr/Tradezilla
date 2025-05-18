using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases
{
    public class DepositUseCase : IDeposit
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepositUseCase(IAccountRepository accountRepository, IAssetRepository assetRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _assetRepository = assetRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task DepositAsync(DepositDto depositDto)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(depositDto.AccountId);
            if (account is null)
            {
                throw new EntityNotFoundException($"Account {depositDto.AccountId} not found");
            }

            var asset = account.Assets.FirstOrDefault(a => a.AssetName == depositDto.AssetName);
            if (asset is null)
            {
                asset = Asset.Create(depositDto.AccountId, depositDto.AssetName);
            }

            var deposit = Deposit.Create(
                asset.AssetId,
                depositDto.Quantity);

            asset.AddDeposit(deposit);

            _assetRepository.Insert(asset);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
