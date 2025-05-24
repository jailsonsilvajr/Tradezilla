using Application.DTOs;

namespace Application.Ports.Driving
{
    public interface IGetDepth
    {
        Task<DepthDto> ExecuteAsync(string marketId, int precision);
    }
}
