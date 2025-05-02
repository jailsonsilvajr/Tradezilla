namespace Application.Ports.Driving
{
    public interface IDeleteAccountUseCase
    {
        Task DeleteAccountByIdAsync(Guid accountId);
    }
}
