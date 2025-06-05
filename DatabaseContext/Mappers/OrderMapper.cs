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
                OrderId = order.GetOrderId(),
                AccountId = order.GetAccountId(),
                Market = order.GetMarket(),
                Side = order.GetSide(),
                Quantity = order.GetQuantity(),
                Price = order.GetPrice(),
                FillQuantity = order.FillQuantity,
                FillPrice = order.FillPrice,
                CreatedAt = order.GetCreatedDate(),
                Status = order.GetStatus()
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
