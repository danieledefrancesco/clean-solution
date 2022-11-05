using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Requests;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OneOf;

namespace AspNetCore.Examples.ProductService.Controllers
{
    public sealed class AppControllerBaseTest
    {
        private AppControllerBase _appControllerBase;
        private IErrorHandlerFactory _errorHandlerFactory;
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _errorHandlerFactory = Substitute.For<IErrorHandlerFactory>();
            _mediator = Substitute.For<IMediator>();
            _appControllerBase = new TestAppControllerBase(_errorHandlerFactory, _mediator);
        }

        [Test]
        public void MediatorResponse_ReturnsObjectResultWithStatusCode200_IfMediatorReturnsSuccessResponse()
        {
            var testRequest = new TestRequest();
            var testResponse = new TestResponse();
            var mediatorResult = Task.FromResult(OneOf<TestResponse,IError>.FromT0(testResponse));
            _mediator.Send(testRequest).Returns(mediatorResult);

            var actionResult = _appControllerBase
                .MediatorResponse<TestRequest, TestResponse>(testRequest, x => x)
                .Result;

            ((ObjectResult) actionResult)
                .StatusCode
                .Should()
                .Be(200);

        }
        
        [Test]
        public void MediatorResponse_ReturnsObjectResultGeneratedByErrorHandler_IfMediatorReturnsErrorResponse()
        {
            var testRequest = new TestRequest();
            var error = Substitute.For<IError>();
            
            var errorActionResult = new ObjectResult(new ())
            {
                StatusCode = 400
            };
            
            var errorHandler = Substitute.For<IErrorHandler>();
            errorHandler
                .HandleError(error)
                .Returns(errorActionResult);

            _errorHandlerFactory
                .GetSupportingHandler(error)
                .Returns(errorHandler);
            
             var mediatorResult = Task.FromResult(OneOf<TestResponse,IError>.FromT1(error));
            _mediator.Send(testRequest).Returns(mediatorResult);

            var actionResult = _appControllerBase
                .MediatorResponse<TestRequest, TestResponse>(testRequest, x => x)
                .Result;

            actionResult
                .Should()
                .Be(errorActionResult);
        }

        private class TestAppControllerBase : AppControllerBase
        {
            public TestAppControllerBase(IErrorHandlerFactory errorHandlerFactory, IMediator mediator) : base(errorHandlerFactory, mediator)
            {
            }
        }

        private class TestResponse
        {
        }

        private class TestRequest : IAppRequest<TestResponse>
        {
        }
    }
}