using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Entities;

namespace Application.UseCases
{
    public class DepositUseCase : IDeposit
    {
        private readonly IDepositRepository _depositRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepositUseCase(IDepositRepository depositRepository, IUnitOfWork unitOfWork)
        {
            _depositRepository = depositRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task DepositAsync(DepositDto depositDto)
        {
            var deposit = Deposit.Create(
                depositDto.AccountId,
                depositDto.AssetId,
                depositDto.Quantity);

            _depositRepository.Insert(deposit);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
