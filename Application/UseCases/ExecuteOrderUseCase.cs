using Application.Ports.Driven;
using Application.Ports.Driving;

namespace Application.UseCases
{
    public class ExecuteOrderUseCase : IExecuteOrder
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ExecuteOrderUseCase(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteOrderAsync(string marketId)
        {
            var orders = await _orderRepository.GetOrdersOpensByMarketIdAsync(marketId);

            var highestBuy = orders?.Where(x => x.GetSide().ToUpper() == "BUY").OrderByDescending(o => o.GetPrice()).FirstOrDefault();
            var lowestSell = orders?.Where(x => x.GetSide().ToUpper() == "SELL").OrderByDescending(o => o.GetPrice()).FirstOrDefault();
            if (highestBuy is null || lowestSell is null || highestBuy.GetPrice() < lowestSell.GetPrice())
            {
                return;
            }

            var fillQuantity = Math.Min(highestBuy.GetQuantity(), lowestSell.GetQuantity());

            highestBuy.SetFillQuantity(fillQuantity);
            lowestSell.SetFillQuantity(fillQuantity);

            _orderRepository.UpdateOrderExecuted(highestBuy);
            _orderRepository.UpdateOrderExecuted(lowestSell);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
