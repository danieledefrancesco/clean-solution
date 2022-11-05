using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Events;
using AspNetCore.Examples.ProductService.OutboxMessages;
using AutoMapper;
using MediatR;
using ZapMicro.TransactionalOutbox.Commands;

namespace AspNetCore.Examples.ProductService.Handlers
{
    public sealed class TransactionalOutboxEventHandler<TEvent, TDto> : INotificationHandler<TEvent> where TEvent : IDomainEvent
    {
        private readonly IEnqueueOutboxMessageCommand _enqueueOutboxMessageCommand;
        private readonly IMapper _mapper;

        public TransactionalOutboxEventHandler(IEnqueueOutboxMessageCommand enqueueOutboxMessageCommand, IMapper mapper)
        {
            _enqueueOutboxMessageCommand = enqueueOutboxMessageCommand;
            _mapper = mapper;
        }

        public async Task Handle(TEvent notification, CancellationToken cancellationToken)
        {
            var outboxMessage = new EventOutboxMessage<TDto>
            {
                Event = _mapper.Map<TDto>(notification)
            };
            await _enqueueOutboxMessageCommand.EnqueueOutboxMessageAsync(outboxMessage, cancellationToken);
        }
    }
}