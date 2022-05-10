
using AspNetCore.Examples.ProductService.ErrorHandlers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Controllers
{
    public class HealthCheckControllerTest
    {
        private HealthCheckController _healthCheckController;
        private IMediator _mediator;
        private IErrorHandlerFactory _errorHandlerFactory;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _errorHandlerFactory = Substitute.For<IErrorHandlerFactory>();
            _healthCheckController = new HealthCheckController(_errorHandlerFactory, _mediator);
        }

        [Test]
        public void HealthCheck_ReturnsOkAlongWithHealthCheckResult()
        {
            var actionResult = _healthCheckController.HealthCheck().Result;
            actionResult.Should().BeOfType<OkObjectResult>();
            
            var objectResult = actionResult as OkObjectResult;
            objectResult!.StatusCode.Should().Be(200);
            objectResult!.Value.Should().BeOfType<HealthCheckDto>();
        }
    }
}