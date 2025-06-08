using Application.Notifications;
using Application.Ports.Driven;
using Application.Ports.Driven.Mediator;

namespace Application.Handlers
{
    public class PlaceTradeHandler : INotificationHandler<PlaceTradeNotification>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlaceTradeHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(PlaceTradeNotification notification)
        {
            _orderRepository.UpdateOrderExecuted(notification.HighestBuy);
            _orderRepository.UpdateOrderExecuted(notification.LowestSell);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
