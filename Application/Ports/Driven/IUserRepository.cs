using Domain.Aggregates;

namespace Application.Ports.Driven
{
    public interface IUserRepository
    {
        void RegisterAccount(User account);
        Task<User?> GetUserByDocumentAsync(string document);
        Task<User?> GetUserByAccountIdAsync(Guid accountId);
    }
}
