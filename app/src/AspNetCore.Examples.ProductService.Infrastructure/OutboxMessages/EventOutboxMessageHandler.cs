using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Events;
using AspNetCore.Examples.ProductService.Handlers;
using ZapMicro.TransactionalOutbox.Handlers;

namespace AspNetCore.Examples.ProductService.OutboxMessages
{
    public class EventOutboxMessageHandler<T>: OutboxMessageHandlerBase<EventOutboxMessage<T>> where T : EventBase
    {
        private readonly IQueueHandler<T> _queueHandler;

        public EventOutboxMessageHandler(IQueueHandler<T> queueHandler)
        {
            _queueHandler = queueHandler;
        }

        public override async ValueTask OnOutboxMessageCreated(EventOutboxMessage<T> outboxMessage, CancellationToken stoppingToken)
        {
            await _queueHandler.SendMessageAsync(outboxMessage.Event, stoppingToken);
        }
    }
}