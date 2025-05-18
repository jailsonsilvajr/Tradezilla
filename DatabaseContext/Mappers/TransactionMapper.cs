using DatabaseContext.Models;
using Domain.Entities;

namespace DatabaseContext.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionModel ToModel(Transaction deposit)
        {
            return new TransactionModel
            {
                DepositId = deposit.TransactionId,
                AssetId = deposit.AssetId,
                Quantity = deposit.Value,
            };
        }

        public static Transaction ToDomain(TransactionModel depositModel)
        {
            return new Transaction(depositModel.DepositId, depositModel.AssetId, depositModel.Quantity);
        }
    }
}
