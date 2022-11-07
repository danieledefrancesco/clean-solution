using System;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Products
{
    public sealed class ProductNameTest
    {
        [TestCase("anything")]
        public void From_ShouldNotThrowException_IfProductNameIsNotNullOrWhiteSpace(string productNameValue)
        {
            Action action = () =>
            {
                ProductName.From(productNameValue); 
                
            };

            action.Should().NotThrow();
        }
        
        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public void From_ShouldThrowArgumentException_IfProductNameIsNotNullOrWhiteSpace(string productNameValue)
        {
            Action action = () =>
            {
                ProductName.From(productNameValue); 
                
            };

            action.Should().Throw<ArgumentException>();
        }
    }
}