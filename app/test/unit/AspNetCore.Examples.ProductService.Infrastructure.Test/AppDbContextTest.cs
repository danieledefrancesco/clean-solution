using System;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService
{
    public sealed class AppDbContextTest
    {
        private string _oldEnvironmentVariableName;

        [SetUp]
        public void SetUp()
        {
            _oldEnvironmentVariableName = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
            Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING","aConnectionString");
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING",_oldEnvironmentVariableName);
        }
        
        [Test]
        public void Constructor_DoesntThrowException()
        {
            Action act = () => new AppDbContext();
            act.Should().NotThrow();
        }
    }
}