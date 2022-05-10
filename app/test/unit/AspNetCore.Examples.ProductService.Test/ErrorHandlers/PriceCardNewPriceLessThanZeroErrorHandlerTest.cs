using AspNetCore.Examples.ProductService.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class PriceCardNewPriceLessThanZeroErrorHandlerTest
    {
        private PriceCardNewPriceLessThanZeroErrorHandler _priceCardNewPriceLessThanZeroErrorHandler;

        [SetUp]
        public void SetUp()
        {
            _priceCardNewPriceLessThanZeroErrorHandler = new PriceCardNewPriceLessThanZeroErrorHandler();
        }

        [Test]
        public void Supports_ReturnsTrue_IfErrorIsAnAlreadyExistsError()
        {
            _priceCardNewPriceLessThanZeroErrorHandler
                .Supports(new PriceCardNewPriceLessThanZeroError())
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void Supports_ReturnsFalse_IfErrorIsNotAnAlreadyExistsError()
        {
            _priceCardNewPriceLessThanZeroErrorHandler
                .Supports(Substitute.For<IError>())
                .Should()
                .BeFalse();
        }
        
        [Test]
        public void HandleError_Returns422ObjectResult()
        {
            var actionResult = _priceCardNewPriceLessThanZeroErrorHandler
                .HandleError(new PriceCardNewPriceLessThanZeroError());
                
            var objectResult = actionResult as ObjectResult;
            objectResult!
                .StatusCode
                .Should()
                .Be(422);
        }
    }
}