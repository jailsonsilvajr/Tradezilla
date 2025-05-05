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
            if (account is null)
            {
                throw new EntityNotFoundException($"Account {accountId} not found");
            }

            return new AccountDto
            {
                Name = account.Name,
                Email = account.Email,
                Document = account.Document,
                Assets = account.Deposits.Select(d => new AssetDto
                {
                    AssetId = d.AssetId,
                    Quantity = d.Quantity
                }).ToList()
            };
        }
    }
}
