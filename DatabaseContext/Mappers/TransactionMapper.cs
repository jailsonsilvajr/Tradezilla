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
                TransactionId = deposit.TransactionId,
                AssetId = deposit.AssetId,
                Quantity = deposit.Quantity,
            };
        }

        public static Transaction ToDomain(TransactionModel depositModel)
        {
            return new Transaction(depositModel.TransactionId, depositModel.AssetId, depositModel.Quantity);
        }
    }
}
