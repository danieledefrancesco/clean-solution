using AspNetCore.Examples.ProductService.Events;
using ZapMicro.TransactionalOutbox.Messages;

namespace AspNetCore.Examples.ProductService.OutboxMessages
{
    public class EventOutboxMessage<T> : IOutboxMessage where T : EventBase
    {
        public T Event { get; set; }
    }
}