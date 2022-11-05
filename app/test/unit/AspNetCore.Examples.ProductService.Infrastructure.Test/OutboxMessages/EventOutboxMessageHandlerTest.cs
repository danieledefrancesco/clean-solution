using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Handlers;
using AspNetCore.Examples.ProductService.QueueHandlers;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.OutboxMessages
{
    public sealed class EventOutboxMessageHandlerTest
    {
        private IQueueHandler<TestDomainEvent> _queueHandler;
        private EventOutboxMessageHandler<TestDomainEvent> _eventOutboxMessageHandler;

        [SetUp]
        public void SetUp()
        {
            _queueHandler = Substitute.For<IQueueHandler<TestDomainEvent>>();
            _eventOutboxMessageHandler = new EventOutboxMessageHandler<TestDomainEvent>(_queueHandler);
        }

        [Test]
        public async Task OnOutboxMessageCreated_DoesntThrowException()
        {
            var outboxMessage = new EventOutboxMessage<TestDomainEvent>
            {
                Event = new TestDomainEvent()
            };
            Func<Task> act = () =>
                _eventOutboxMessageHandler.OnOutboxMessageCreated(outboxMessage, CancellationToken.None).AsTask();

            await act.Should().NotThrowAsync();
        }
    }
}