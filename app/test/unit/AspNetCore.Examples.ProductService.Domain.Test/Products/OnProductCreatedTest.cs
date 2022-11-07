using System;
using AspNetCore.Examples.ProductService.Events;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Products
{
    public class OnProductCreatedTest
    {
        [Test]
        public void Getters_ShouldReturnConstructionArguments()
        {
            var id = Guid.NewGuid();
            var productId = ProductId.From("id");
            var productName = ProductName.From("name");
            var productPrice = ProductPrice.From(1);
            var onProductCreated = new OnProductCreated(id, productId, productName, productPrice);

            onProductCreated.Id.Should().Be(id);
            onProductCreated.ProductId.Should().Be(productId);
            onProductCreated.ProductName.Should().Be(productName);
            onProductCreated.ProductPrice.Should().Be(productPrice);
        }
    }
}