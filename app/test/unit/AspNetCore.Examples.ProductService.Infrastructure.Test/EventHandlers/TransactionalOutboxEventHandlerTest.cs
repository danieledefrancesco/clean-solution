using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Events;
using AspNetCore.Examples.ProductService.Handlers;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.Commands;

namespace AspNetCore.Examples.ProductService.EventHandlers
{
    public sealed class TransactionalOutboxEventHandlerTest : DomainEventBase
    {
        private IEnqueueOutboxMessageCommand _enqueueOutboxMessageCommand;
        private IMapper _mapper;
        private TransactionalOutboxEventHandler<TestDomainEvent, TestDomainEventDto> _transactionalOutboxEventHandler;

        [SetUp]
        public void SetUp()
        {
            _enqueueOutboxMessageCommand = Substitute.For<IEnqueueOutboxMessageCommand>();
            _mapper = Substitute.For<IMapper>();
            _transactionalOutboxEventHandler = new TransactionalOutboxEventHandler<TestDomainEvent, TestDomainEventDto>(_enqueueOutboxMessageCommand, _mapper);
        }

        [Test]
        public async Task RaiseEvent_DoesntThrowException()
        {
            var @event = new TestDomainEvent();
            Func<Task> act = () => _transactionalOutboxEventHandler.Handle(@event, CancellationToken.None);
            await act.Should().NotThrowAsync();
        }
    }
}