using System;
using System.IO;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Result;
using Microsoft.Extensions.Logging;
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
        public async Task HandleError_ReturnsObjectResultWithExpectedStatusCode()
        {
            var actionResult = ErrorHandler
                .HandleError(CreateErrorInstance());
            var loggerFactory = Substitute.For<ILoggerFactory>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(ILoggerFactory)).Returns(loggerFactory);
            var response = Substitute.For<HttpResponse>();
            var stream = new MemoryStream();
            response.Body.Returns(stream);
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Response.Returns(response);
            httpContext.RequestServices.Returns(serviceProvider);
            await actionResult.ExecuteAsync(httpContext);
            response.StatusCode.Should().Be(ExpectedStatusCode);
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