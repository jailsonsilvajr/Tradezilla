using Application.Notifications;
using Application.Ports.Driven.Mediator;
using Application.Ports.Driving;

namespace Application.Handlers
{
    public class PlaceOrderHandler : INotificationHandler<PlaceOrderNotification>
    {
        private readonly IExecuteOrder _executeOrder;

        public PlaceOrderHandler(IExecuteOrder executeOrder)
        {
            _executeOrder = executeOrder;
        }

        public async Task Handle(PlaceOrderNotification request)
        {
            await _executeOrder.ExecuteOrderAsync(request.Order.Market!);
        }
    }
}
