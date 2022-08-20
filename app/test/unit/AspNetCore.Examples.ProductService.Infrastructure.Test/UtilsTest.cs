using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService
{
    public class UtilsTest
    {
        [Test]
        public async Task CreateAzureStorageQueuesIfDontExist_DoesntThrowException()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var queue = Substitute.For<QueueClient>();
            var queueFactories = new List<Func<QueueClient>>()
            {
                () => queue
            };
            serviceProvider.GetService(typeof(IEnumerable<Func<QueueClient>>)).Returns(queueFactories);
            Func<Task> act = () => Utils.CreateAzureStorageQueuesIfDontExist(serviceProvider);
            await act.Should().NotThrowAsync();
        }
    }
}