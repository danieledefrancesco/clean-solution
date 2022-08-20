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
    public class AzureStorageQueueHandlerTest
    {
        private QueueClient _queueClient;
        private AzureStorageQueueHandler<TestEvent> _queueHandler;

        [SetUp]
        public void SetUp()
        {
            _queueClient = Substitute.For<QueueClient>();
            _queueHandler = new AzureStorageQueueHandler<TestEvent>(_queueClient);
        }

        [Test]
        public async Task SendMessageAsync_DoesntThrowException()
        {
            var @event = new TestEvent();
            Func<Task> act = () => _queueHandler.SendMessageAsync(@event, CancellationToken.None);
            await act.Should().NotThrowAsync();
        }
    }
}