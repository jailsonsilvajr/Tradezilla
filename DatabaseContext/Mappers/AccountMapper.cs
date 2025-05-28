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
                AccountId = account.AccountId,
                Name = account.GetName(),
                Email = account.GetEmail(),
                Document = account.GetDocument(),
                Password = account.Password,
                Assets = account.Assets.Select(a => AssetMapper.ToModel(a)).ToList(),
                Orders = account.Orders.Select(o => OrderMapper.ToModel(o)).ToList()
            };
        }

        public static Account ToDomain(AccountModel accountModel)
        {
            var assets = accountModel.Assets.Select(a => AssetMapper.ToDomain(a)).ToList();
            var orders = accountModel.Orders.Select(o => OrderMapper.ToDomain(o)).ToList();
            return new Account(accountModel.AccountId, accountModel.Name, accountModel.Email, accountModel.Document, accountModel.Password, assets, orders);
        }
    }
}
