namespace Application.Ports.Driven.Mediator
{
    public interface IMediator
    {
        Task Send<TNotification>(TNotification notification);
    }
}
