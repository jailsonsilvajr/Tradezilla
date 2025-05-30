using Application.DTOs;
using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.Exceptions;

namespace Application.UseCases
{
    public class GetAccountUseCase : IGetAccount
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountUseCase(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountDto> GetAccountByAccountIdAsync(Guid accountId)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(accountId);
            return account is null
                ? throw new EntityNotFoundException($"Account {accountId} not found")
                : new AccountDto
                {
                    Name = account.GetName(),
                    Email = account.GetEmail(),
                    Document = account.GetDocument(),
                    Assets = account.Assets.Select(d => new AssetDto
                    {
                        AssetName = d.AssetName,
                        Balance = d.Balance
                    }).ToList()
                };
        }
    }
}
