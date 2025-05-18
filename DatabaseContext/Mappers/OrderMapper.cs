using DatabaseContext.Models;
using Domain.Entities;

namespace DatabaseContext.Mappers
{
    public static class OrderMapper
    {
        public static OrderModel ToModel(Order order)
        {
            return new OrderModel
            {
                OrderId = order.OrderId,
                AccountId = order.AccountId,
                Market = order.Market,
                Side = order.Side,
                Quantity = order.Quantity,
                Price = order.Price,
                FillQuantity = order.FillQuantity,
                FillPrice = order.FillPrice,
                CreatedAt = order.CreatedAt,
                Status = order.Status
            };
        }

        public static Order ToDomain(OrderModel orderModel)
        {
            return new Order(
                orderModel.OrderId,
                orderModel.AccountId,
                orderModel.Market,
                orderModel.Side,
                orderModel.Quantity,
                orderModel.Price,
                orderModel.CreatedAt,
                orderModel.Status);
        }
    }
}
