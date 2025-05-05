using Domain.Entities;

namespace Application.Ports.Driven
{
    public interface IDepositRepository
    {
        void Insert(Deposit deposit);
    }
}
