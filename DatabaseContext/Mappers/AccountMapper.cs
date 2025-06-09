using DatabaseContext.Models;
using Domain.Entities;

namespace DatabaseContext.Mappers
{
    public static class AccountMapper
    {
        public static AccountModel ToModel(Account account)
        {
            return new AccountModel
            {
                AccountId = account.GetId(),
                Name = account.GetName(),
                Email = account.GetEmail(),
                Document = account.GetDocument(),
                Password = account.GetPassword()
            };
        }

        public static Account ToDomain(AccountModel accountModel)
        {
            return new Account(accountModel.AccountId, accountModel.Name, accountModel.Email, accountModel.Document, accountModel.Password);
        }
    }
}
