using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.GetProductWithPriceCardById
{
    public sealed class GetProductWithPriceCardRequestDtoValidatorTest
    {
        private GetProductWithPriceCardByIdRequestDtoValidator _getProductWithPriceCardByIdRequestDtoValidator;

        [SetUp]
        public void SetUp()
        {
            _getProductWithPriceCardByIdRequestDtoValidator = new GetProductWithPriceCardByIdRequestDtoValidator();
        }

        [TestCase("a1")]
        [TestCase("a2")]
        [TestCase("A1")]
        [TestCase("A-902_ sb")]
        public void GetProductDTORequestValidator_ThrowsNoError_IfProductIdIsValid(string id)
        {
            var getProductDtoRequest = new GetProductWithPriceCardByIdRequestDto()
            {
                ProductId = id
            };

            var validationResult = _getProductWithPriceCardByIdRequestDtoValidator.Validate(getProductDtoRequest);

            validationResult
                .Should()
                .NotBeNull();

            var errors = validationResult.Errors;
            errors
                .Should()
                .BeEmpty();
        }
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase("1A")]
        [TestCase("-abc")]
        [TestCase("_abc")]
        [TestCase(" abc")]
        [TestCase(" ")]
        public void GetProductDTORequestValidator_ThrowsError_IfProductIdIsInvalid(string id)
        {
            var getProductDtoRequest = new GetProductWithPriceCardByIdRequestDto()
            {
                ProductId = id
            };

            var validationResult = _getProductWithPriceCardByIdRequestDtoValidator.Validate(getProductDtoRequest);

            validationResult
                .Should()
                .NotBeNull();

            var errors = validationResult.Errors;
            errors
                .Should()
                .NotBeEmpty();
        }
    }
}