using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Exceptions;

namespace Application.UseCases
{
    public class GetUserUseCase : IGetUser
    {
        private readonly IUserRepository _userRepository;

        public GetUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetUserByAccountIdAsync(Guid accountId)
        {
            var user = await _userRepository.GetUserByAccountIdAsync(accountId);
            return user is null
                ? throw new EntityNotFoundException($"User with AccountId {accountId} not found")
                : new UserDto
                {
                    Name = user.GetName(),
                    Email = user.GetEmail(),
                    Document = user.GetDocument()
                };
        }
    }
}
