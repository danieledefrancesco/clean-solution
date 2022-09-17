using AspNetCore.Examples.ProductService.Endpoints;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService
{
    public class DependencyInjectionTests
    {
        [Test]
        public void GetConcreteEndpointTypes_ReturnsConcreteEndpointTypes()
        {
            DependencyInjectionExtensions
                .GetConcreteEndpointTypes()
                .Should()
                .HaveCount(2)
                .And
                .Contain(new [] {typeof(CreateProductEndpoint), typeof(GetProductEndpoint)});
        }

        [Test]
        public void CreateEndpointInstance_ReturnsNotNullObject()
        {
            DependencyInjectionExtensions
                .CreateEndpointInstance(typeof(CreateProductEndpoint))
                .Should()
                .NotBeNull();
        }
    }
}