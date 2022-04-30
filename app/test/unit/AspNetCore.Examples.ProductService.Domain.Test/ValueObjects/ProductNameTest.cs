using System;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ValueObjects
{
    public class ProductNameTest
    {
        [TestCase("anything")]
        public void Test_ValidProductName_ShouldNotThrowException(string productNameValue)
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
        public void Test_InvalidProductName_ShouldThrowArgumentException(string productNameValue)
        {
            Action action = () =>
            {
                ProductName.From(productNameValue); 
                
            };

            action.Should().Throw<ArgumentException>();
        }
    }
}