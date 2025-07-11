﻿using DatabaseContext.Models;
using Domain.Aggregates;

namespace DatabaseContext.Mappers
{
    public static class WalletMapper
    {
        public static Wallet ToDomain(AccountModel accountModel)
        {
            var assets = accountModel.Assets.Select(AssetMapper.ToDomain).ToList();
            var orders = accountModel.Orders.Select(OrderMapper.ToDomain).ToList();
            var transactions = accountModel.Assets.SelectMany(x => x.Transactions.Select(y => TransactionMapper.ToDomain(y))).ToList();
            return new Wallet(accountModel.AccountId, assets, orders, transactions);
        }
    }
}
