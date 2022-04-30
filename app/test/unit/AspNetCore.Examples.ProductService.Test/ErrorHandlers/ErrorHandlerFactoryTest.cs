using System.Collections.Generic;
using AspNetCore.Examples.ProductService.Errors;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class ErrorHandlerFactoryTest
    {
        [Test]
        public void GetSupportingHandler_ReturnsAnHandler_IfSupportingHandlerExists()
        {
            var errorHandler = Substitute.For<IErrorHandler>();
            
            errorHandler
                .Supports(Arg.Any<IError>())
                .Returns(true);

            var handlersList = new List<IErrorHandler>()
            {
                errorHandler
            };

            var errorHandlerFactory = new ErrorHandlerFactory(handlersList);

            var error = Substitute.For<IError>();

            errorHandlerFactory
                .GetSupportingHandler(error)
                .Should()
                .Be(errorHandler);


        }
    }
}