using Application.Ports.Driven;
using Domain.Entities;

namespace DatabaseContext.Repositories
{
    public class DepositRepository : IDepositRepository
    {
        private readonly TradezillaContext _context;

        public DepositRepository(TradezillaContext context)
        {
            _context = context;
        }

        public void Insert(Deposit deposit)
        {
            _context.Deposits.Add(deposit);
        }
    }
}
