using AspNetCore.Examples.ProductService.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class NotFoundErrorHandlerTest
    {
        private NotFoundErrorHandler _notFoundErrorHandler;

        [SetUp]
        public void SetUp()
        {
            _notFoundErrorHandler = new NotFoundErrorHandler();
        }

        [Test]
        public void Supports_ReturnsTrue_IfErrorIsANotFoundError()
        {
            _notFoundErrorHandler
                .Supports(new NotFoundError())
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void Supports_ReturnsFalse_IfErrorIsNotANotFoundError()
        {
            _notFoundErrorHandler
                .Supports(Substitute.For<IError>())
                .Should()
                .BeFalse();
        }
        
        [Test]
        public void HandleError_Returns404ObjectResult()
        {
            var actionResult = _notFoundErrorHandler
                .HandleError(new NotFoundError());
                
            var objectResult = actionResult as ObjectResult;
            objectResult
                .StatusCode
                .Should()
                .Be(404);
        }
    }
}