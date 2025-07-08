using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Account
    {
        private readonly ID _id;

        public Account(Guid accountId)
        {
            _id = new ID(accountId);
        }

        public Guid GetId() => _id.GetValue();
    }
}
