using DatabaseContext.Models;
using Domain.Aggregates;

namespace DatabaseContext.Mappers
{
    public static class AccountMapper
    {
        public static AccountModel ToModel(User account)
        {
            return new AccountModel
            {
                AccountId = account.GetAccountId(),
                Name = account.GetName(),
                Email = account.GetEmail(),
                Document = account.GetDocument(),
                Password = account.GetPassword()
            };
        }

        public static User ToDomain(AccountModel accountModel)
        {
            return new User(accountModel.AccountId, accountModel.Name, accountModel.Email, accountModel.Document, accountModel.Password);
        }
    }
}
