using Domain.Entities;

namespace Application.Ports.Driven
{
    public interface IAccountRepository
    {
        Task RegisterAccountAsync(Account account);
    }
}
