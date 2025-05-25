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

            var highestBuy = orders?.Where(x => x.Side!.ToUpper() == "BUY").OrderByDescending(o => o.Price).FirstOrDefault();
            var lowestSell = orders?.Where(x => x.Side!.ToUpper() == "SELL").OrderByDescending(o => o.Price).FirstOrDefault();
            if (highestBuy is null || lowestSell is null || highestBuy.Price < lowestSell.Price)
            {
                return;
            }

            var fillQuantity = Math.Min(highestBuy.Quantity, lowestSell.Quantity);
            var fillPrice = highestBuy.CreatedAt > lowestSell.CreatedAt ? lowestSell.Price : highestBuy.Price;
            var tradeSide = highestBuy.CreatedAt > lowestSell.CreatedAt ? "BUY" : "SELL";

            highestBuy.SetFillQuantity(fillQuantity);
            lowestSell.SetFillQuantity(fillQuantity);

            _orderRepository.UpdateOrderExecuted(highestBuy);
            _orderRepository.UpdateOrderExecuted(lowestSell);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
