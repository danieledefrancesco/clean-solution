using AspNetCore.Examples.ProductService.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class AlreadyExistsErrorHandlerTest
    {
        private AlreadyExistsErrorHandler _alreadyExistsErrorHandler;

        [SetUp]
        public void SetUp()
        {
            _alreadyExistsErrorHandler = new AlreadyExistsErrorHandler();
        }

        [Test]
        public void Supports_ReturnsTrue_IfErrorIsAnAlreadyExistsError()
        {
            _alreadyExistsErrorHandler
                .Supports(new AlreadyExistsError())
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void Supports_ReturnsFalse_IfErrorIsNotAnAlreadyExistsError()
        {
            _alreadyExistsErrorHandler
                .Supports(Substitute.For<IError>())
                .Should()
                .BeFalse();
        }
        
        [Test]
        public void HandleError_Returns409ObjectResult()
        {
            var actionResult = _alreadyExistsErrorHandler
                .HandleError(new AlreadyExistsError());
                
            var objectResult = actionResult as ObjectResult;
            objectResult
                .StatusCode
                .Should()
                .Be(409);
        }
    }
}