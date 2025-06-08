using Application.DTOs;
using Application.Notifications;
using Application.Ports.Driven;
using Application.Ports.Driven.Mediator;
using Application.Ports.Driving;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases
{
    public class PlaceOrderUseCase : IPlaceOrder
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IBook _book;

        public PlaceOrderUseCase(
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            IBook book)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _book = book;
        }

        public async Task<Guid> PlaceOrder(PlaceOrderDto placeOrderDto)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(placeOrderDto.AccountId);
            if (account == null)
            {
                throw new EntityNotFoundException($"AccountId {placeOrderDto.AccountId} not found");
            }

            var order = Order.Create(
                account.GetId(),
                placeOrderDto.MarketId,
                placeOrderDto.Side,
                placeOrderDto.Quantity,
                placeOrderDto.Price);

            account.AddOrder(order);

            await _accountRepository.RegisterOrdersAsync(account);
            await _unitOfWork.SaveChangesAsync();

            _book.AddOrder(order);
            await _mediator.Send(new PlaceOrderNotification(order));

            return order.GetOrderId();
        }
    }
}
