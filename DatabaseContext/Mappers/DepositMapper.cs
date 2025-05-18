using DatabaseContext.Models;
using Domain.Entities;

namespace DatabaseContext.Mappers
{
    public static class DepositMapper
    {
        public static DepositModel ToModel(Deposit deposit)
        {
            return new DepositModel
            {
                DepositId = deposit.DepositId,
                AssetId = deposit.AssetId,
                Quantity = deposit.Quantity,
            };
        }

        public static Deposit ToDomain(DepositModel depositModel)
        {
            return new Deposit(depositModel.DepositId, depositModel.AssetId, depositModel.Quantity);
        }
    }
}
