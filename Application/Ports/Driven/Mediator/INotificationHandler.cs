namespace Application.Ports.Driven.Mediator
{
    public interface INotificationHandler<TNotification> where TNotification : INotification
    {
        Task Handle(TNotification notification);
    }
}
