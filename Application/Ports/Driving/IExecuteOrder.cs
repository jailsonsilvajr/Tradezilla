namespace Application.Ports.Driving
{
    public interface IExecuteOrder
    {
        Task ExecuteOrderAsync(string marketId);
    }
}
