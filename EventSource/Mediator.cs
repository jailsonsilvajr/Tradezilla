using Application.Ports.Driven.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace EventSource
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Send<TNotification>(TNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));
            var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());

            var handlers = _serviceProvider.GetServices(handlerType);
            ArgumentNullException.ThrowIfNull(handlers, nameof(handlers));

            if (handlers.Any(h => h is null))
            {
                throw new ArgumentNullException(nameof(handlers), "One or more handlers are null");
            }

            var tasks = handlers.Select(handler => (Task)((dynamic)handler!).Handle((dynamic)notification)).ToArray();
            await Task.WhenAll(tasks);
        }
    }
}
