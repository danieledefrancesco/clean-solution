using AspNetCore.Examples.ProductService.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class DefaultErrorHandlerTest
    {
        private DefaultErrorHandler _defaultErrorHandler;

        [SetUp]
        public void SetUp()
        {
            _defaultErrorHandler = new DefaultErrorHandler();
        }

        [Test]
        public void Supports_ReturnsTrue()
        {
            _defaultErrorHandler
                .Supports(Substitute.For<IError>())
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void HandleError_Returns400ObjectResult()
        {
            var actionResult = _defaultErrorHandler
                .HandleError(new AlreadyExistsError());
                
            var objectResult = actionResult as ObjectResult;
            objectResult
                .StatusCode
                .Should()
                .Be(400);
        }
    }
}