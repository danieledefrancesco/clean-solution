using System;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService
{
    public class ProgramTest
    {
        [Test]
        public void CreateHostBuilder_ShouldNotThrowException()
        {
            Action createHostBuilderAction = () =>
            {
                Program
                    .CreateHostBuilder(Array.Empty<string>())
                    .Build();
            };
            
            createHostBuilderAction
                .Should()
                .NotThrow();
        }
    }
}