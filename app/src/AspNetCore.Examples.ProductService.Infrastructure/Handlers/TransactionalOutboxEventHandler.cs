using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Events;
using AspNetCore.Examples.ProductService.OutboxMessages;
using ZapMicro.TransactionalOutbox.Commands;

namespace AspNetCore.Examples.ProductService.Handlers
{
    public class TransactionalOutboxEventHandler : IEventHandler
    {
        private readonly IEnqueueOutboxMessageCommand _enqueueOutboxMessageCommand;

        public TransactionalOutboxEventHandler(IEnqueueOutboxMessageCommand enqueueOutboxMessageCommand)
        {
            _enqueueOutboxMessageCommand = enqueueOutboxMessageCommand;
        }

        public async Task RaiseEvent<T>(T @event) where T : EventBase
        {
            var outboxMessage = new EventOutboxMessage<T>
            {
                Event = @event
            };
            await _enqueueOutboxMessageCommand.EnqueueOutboxMessageAsync(outboxMessage, CancellationToken.None);
        }
    }
}