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
    public class EventOutboxMessageHandlerTest
    {
        private IQueueHandler<TestEvent> _queueHandler;
        private EventOutboxMessageHandler<TestEvent> _eventOutboxMessageHandler;

        [SetUp]
        public void SetUp()
        {
            _queueHandler = Substitute.For<IQueueHandler<TestEvent>>();
            _eventOutboxMessageHandler = new EventOutboxMessageHandler<TestEvent>(_queueHandler);
        }

        [Test]
        public async Task OnOutboxMessageCreated_DoesntThrowException()
        {
            var outboxMessage = new EventOutboxMessage<TestEvent>
            {
                Event = new TestEvent()
            };
            Func<Task> act = () =>
                _eventOutboxMessageHandler.OnOutboxMessageCreated(outboxMessage, CancellationToken.None).AsTask();

            await act.Should().NotThrowAsync();
        }
    }
}