using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Handlers
{
    public sealed class AzureStorageQueueHandlerTest
    {
        private QueueClient _queueClient;
        private AzureStorageQueueHandler<TestDomainEvent> _queueHandler;

        [SetUp]
        public void SetUp()
        {
            _queueClient = Substitute.For<QueueClient>();
            var factory = Substitute.For<IAzureStorageQueueClientFactory<TestDomainEvent>>();
            factory.Create().Returns(_queueClient);
            _queueHandler = new AzureStorageQueueHandler<TestDomainEvent>(factory);
        }

        [Test]
        public async Task SendMessageAsync_DoesntThrowException()
        {
            var @event = new TestDomainEvent(Guid.NewGuid());
            Func<Task> act = () => _queueHandler.SendMessageAsync(@event, CancellationToken.None);
            await act.Should().NotThrowAsync();
        }
    }
}