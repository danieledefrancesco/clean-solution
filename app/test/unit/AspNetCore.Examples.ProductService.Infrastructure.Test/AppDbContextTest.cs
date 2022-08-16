using System;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService
{
    public class AppDbContextTest
    {
        [Test]
        public void Constructor_DoesntThrowException()
        {
            Action act = () => new AppDbContext();
            act.Should().NotThrow();
        }
    }
}