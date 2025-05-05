using Domain.Entities;

namespace Application.Ports.Driven
{
    public interface IAccountRepository
    {
        void RegisterAccount(Account account);
        Task<Account?> GetAccountByDocumentAsync(string document);
        Task<Account?> GetAccountByAccountIdAsync(Guid accountId);
    }
}
