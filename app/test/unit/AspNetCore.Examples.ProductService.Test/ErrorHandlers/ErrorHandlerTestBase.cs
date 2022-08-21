using AspNetCore.Examples.ProductService.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public abstract class ErrorHandlerTestBase<T> where T: IErrorHandler
    {
        protected abstract T ErrorHandler { get; set; }
        protected abstract int ExpectedStatusCode { get; }
        protected abstract IError CreateErrorInstance();
        [Test]
        public void HandleError_ReturnsObjectResultWithExpectedStatusCode()
        {
            var actionResult = ErrorHandler
                .HandleError(CreateErrorInstance());

            var objectResult = actionResult as ObjectResult;
            objectResult!
                .StatusCode
                .Should()
                .Be(ExpectedStatusCode);
        }
    }
    public abstract class ErrorHandlerTestBase<TErrorHandler, TError>: ErrorHandlerTestBase<TErrorHandler> 
        where TError: IError
        where TErrorHandler: ErrorHandlerBase<TError>
    {

        [Test]
        public void Supports_ReturnsTrue_IfErrorIsOfSupportedType()
        {
            ErrorHandler
                .Supports(CreateErrorInstance())
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void Supports_ReturnsFalse_IfErrorIsNotOfSupportedType()
        {
            ErrorHandler
                .Supports(Substitute.For<IError>())
                .Should()
                .BeFalse();
        }
    }
}