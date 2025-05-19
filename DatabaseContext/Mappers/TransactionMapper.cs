using DatabaseContext.Models;
using Domain.Entities;
using Domain.Enums;

namespace DatabaseContext.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionModel ToModel(Transaction transaction)
        {
            return new TransactionModel
            {
                TransactionId = transaction.TransactionId,
                AssetId = transaction.AssetId,
                Quantity = transaction.Quantity,
                TransactionType = (int)transaction.TransactionType
            };
        }

        public static Transaction ToDomain(TransactionModel transactionModel)
        {
            return new Transaction(transactionModel.TransactionId, transactionModel.AssetId, transactionModel.Quantity, (TransactionType)transactionModel.TransactionType);
        }
    }
}
