using Application.DTOs;

namespace Application.Ports.Driving
{
    public interface IGetUser
    {
        Task<UserDto> GetUserByAccountIdAsync(Guid accountId);
    }
}
