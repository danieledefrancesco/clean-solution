using System;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService
{
    public class ProgramTest
    {
        [Test]
        public void CreateHostBuilder_DoesntThrowException()
        {
            Action createHostBuilderAction = () =>
            {
                Program.CreateHostBuilder(new string[] { });
            };
            
            createHostBuilderAction
                .Should()
                .NotThrow();
        }
    }
}