using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases
{
    public class PlaceOrderUseCase : IPlaceOrder
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExecuteOrder _executeOrder;

        public PlaceOrderUseCase(
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IExecuteOrder executeOrder)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _executeOrder = executeOrder;
        }

        public async Task<Guid> PlaceOrder(PlaceOrderDto placeOrderDto)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(placeOrderDto.AccountId);
            if (account == null)
            {
                throw new EntityNotFoundException($"AccountId {placeOrderDto.AccountId} not found");
            }

            var order = Order.Create(
                account.AccountId,
                placeOrderDto.MarketId,
                placeOrderDto.Side,
                placeOrderDto.Quantity,
                placeOrderDto.Price);

            account.AddOrder(order);

            await _accountRepository.RegisterOrdersAsync(account);
            await _unitOfWork.SaveChangesAsync();

            await _executeOrder.ExecuteOrderAsync(placeOrderDto.MarketId!);

            return order.OrderId;
        }
    }
}
