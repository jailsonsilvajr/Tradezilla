using Application.DTOs;
using Application.Notifications;
using Application.Ports.Driven;
using Application.Ports.Driven.Mediator;
using Application.Ports.Driving;
using Domain.Entities;

namespace Application.UseCases
{
    public class PlaceOrderUseCase : IPlaceOrder
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IBook _book;

        public PlaceOrderUseCase(
            IWalletRepository walletRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            IBook book)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _book = book;
        }

        public async Task<Guid> PlaceOrder(PlaceOrderDto placeOrderDto)
        {
            var wallet = await _walletRepository.GetWalletByAccountIdAsync(placeOrderDto.AccountId);

            var order = Order.Create(
                wallet.GetAccountId(),
                placeOrderDto.MarketId,
                placeOrderDto.Side,
                placeOrderDto.Quantity,
                placeOrderDto.Price);

            wallet.AddOrder(order);

            await _walletRepository.RegisterOrdersAsync(wallet);
            await _unitOfWork.SaveChangesAsync();

            _book.AddOrder(order);
            await _mediator.Send(new PlaceOrderNotification(order));

            return order.GetOrderId();
        }
    }
}
