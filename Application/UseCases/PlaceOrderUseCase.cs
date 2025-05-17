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
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlaceOrderUseCase(
            IAccountRepository accountRepository,
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
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

            var assetName = order.Side?.ToUpper() != "BUY"
                ? order.Market?.Split("/")[0].ToUpper()
                : order.Market?.Split("/")[1].ToUpper();

            var asset = account.Assets
                .FirstOrDefault(asset => asset.AssetName?.ToUpper() == assetName);

            if (asset is null)
            {
                throw new EntityNotFoundException($"Asset {assetName} not found");
            }

            if (asset.Balance < order.Quantity)
            {
                throw new InsufficientBalanceException($"Insufficient balance for asset {assetName}");
            }

            _orderRepository.RegisterOrder(order);
            await _unitOfWork.SaveChangesAsync();

            return order.OrderId;
        }
    }
}
