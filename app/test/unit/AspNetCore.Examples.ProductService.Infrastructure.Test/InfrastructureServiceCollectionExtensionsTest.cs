using System;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService
{
    public sealed class InfrastructureServiceCollectionExtensionsTest
    {
        [Test]
        public void AddInfrastructureLayer_ShouldNotThrow()
        {
            var services = new ServiceCollection();
            var configuration = Substitute.For<IConfiguration>();
            var action = (Action)(() => services.AddInfrastructureLayer(configuration));
            action.Should().NotThrow();
        }
    }
}