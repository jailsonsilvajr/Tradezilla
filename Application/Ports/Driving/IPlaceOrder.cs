using Application.DTOs;

namespace Application.Ports.Driving
{
    public interface IPlaceOrder
    {
        Task<Guid> PlaceOrder(PlaceOrderDto placeOrderDto);
    }
}
