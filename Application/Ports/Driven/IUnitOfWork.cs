namespace Application.Ports.Driven
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
