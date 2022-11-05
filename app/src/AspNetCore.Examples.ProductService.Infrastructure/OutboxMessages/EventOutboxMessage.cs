using ZapMicro.TransactionalOutbox.Messages;

namespace AspNetCore.Examples.ProductService.OutboxMessages
{
    public sealed class EventOutboxMessage<T> : IOutboxMessage
    {
        public T Event { get; init; }
    }
}