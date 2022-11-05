using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Handlers;
using Azure.Storage.Queues;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.QueueHandlers
{
    public sealed class AzureStorageQueueHandlerTest
    {
        private QueueClient _queueClient;
        private AzureStorageQueueHandler<TestDomainEvent> _queueHandler;

        [SetUp]
        public void SetUp()
        {
            _queueClient = Substitute.For<QueueClient>();
            _queueHandler = new AzureStorageQueueHandler<TestDomainEvent>(_queueClient);
        }

        [Test]
        public async Task SendMessageAsync_DoesntThrowException()
        {
            var @event = new TestDomainEvent();
            Func<Task> act = () => _queueHandler.SendMessageAsync(@event, CancellationToken.None);
            await act.Should().NotThrowAsync();
        }
    }
}