using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Events;
using AspNetCore.Examples.ProductService.OutboxMessages;
using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.Commands;

namespace AspNetCore.Examples.ProductService.Handlers
{
    public class TransactionalOutboxEventHandler : IEventHandler
    {
        private readonly IEnqueueOutboxMessageCommand _enqueueOutboxMessageCommand;
        private readonly DbContext _dbContext;

        public TransactionalOutboxEventHandler(IEnqueueOutboxMessageCommand enqueueOutboxMessageCommand, DbContext dbContext)
        {
            _enqueueOutboxMessageCommand = enqueueOutboxMessageCommand;
            _dbContext = dbContext;
        }

        public async Task RaiseEvent<T>(T @event) where T : EventBase
        {
            var outboxMessage = new EventOutboxMessage<T>
            {
                Event = @event
            };
            await _enqueueOutboxMessageCommand.EnqueueOutboxMessageAsync(outboxMessage, CancellationToken.None);
            await _dbContext.SaveChangesAsync();
        }
    }
}