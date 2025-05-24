using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Entities;

namespace Application.UseCases
{
    public class GetDepthUseCase : IGetDepth
    {
        private readonly IOrderRepository _orderRepository;

        public GetDepthUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<DepthDto> ExecuteAsync(string marketId, int precision)
        {
            var orders = await _orderRepository.GetOrdersByMarketIdAsync(marketId);
            var index = Order.GroupOrdersByPrecision(orders, precision);

            var depthDto = new DepthDto();
            foreach (var price in index.buy)
            {
                depthDto.Buys.Add(new BuyDepthDto
                {
                    Price = decimal.Parse(price.Key),
                    Quantity = price.Value
                });
            }

            foreach (var price in index.sell)
            {
                depthDto.Sells.Add(new SellDepthDto
                {
                    Price = decimal.Parse(price.Key),
                    Quantity = price.Value
                });
            }

            return depthDto;
        }
    }
}
