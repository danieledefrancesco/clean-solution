using System;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Handlers;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.Commands;

namespace AspNetCore.Examples.ProductService.EventHandlers
{
    public class TransactionalOutboxEventHandlerTest
    {
        private IEnqueueOutboxMessageCommand _enqueueOutboxMessageCommand;
        private TransactionalOutboxEventHandler _transactionalOutboxEventHandler;

        [SetUp]
        public void SetUp()
        {
            _enqueueOutboxMessageCommand = Substitute.For<IEnqueueOutboxMessageCommand>();
            _transactionalOutboxEventHandler = new TransactionalOutboxEventHandler(_enqueueOutboxMessageCommand);
        }

        [Test]
        public async Task RaiseEvent_DoesntThrowException()
        {
            var @event = new TestEvent();
            Func<Task> act = () => _transactionalOutboxEventHandler.RaiseEvent(@event);
            await act.Should().NotThrowAsync();
        }
    }
}