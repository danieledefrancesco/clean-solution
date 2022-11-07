using System;
using AspNetCore.Examples.ProductService.Products;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ValueObjects
{
    public sealed class ProductPriceTest
    {
        [TestCase(1)]
        [TestCase(0)]
        public void From_ShouldNotThrowException_IfPriceIsGreatherOrEqualToZero(decimal price)
        {
            Action action = () => { ProductPrice.From(price); };
            action.Should().NotThrow();
        }
        
        [TestCase(-1)]

        public void From_ShouldThrowArgumentException_IfPriceIsLessThanZero(decimal price)
        {
            Action action = () => { ProductPrice.From(price); };
            action.Should().Throw<ArgumentException>();
        }
    }
}