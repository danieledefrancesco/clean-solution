using AspNetCore.Examples.ProductService.DataTransferObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.GetProductById
{
    public sealed class GetProductRequestDtoValidatorTest
    {
        private GetProductByIdRequestDtoValidator _getProductByIdRequestDtoValidator;

        [SetUp]
        public void SetUp()
        {
            _getProductByIdRequestDtoValidator = new GetProductByIdRequestDtoValidator();
        }

        [TestCase("a1")]
        [TestCase("a2")]
        [TestCase("A1")]
        [TestCase("A-902_ sb")]
        public void GetProductDTORequestValidator_ThrowsNoError_IfProductIdIsValid(string id)
        {
            var getProductDtoRequest = new GetProductByIdRequestDto
            {
                ProductId = id
            };

            var validationResult = _getProductByIdRequestDtoValidator.Validate(getProductDtoRequest);

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
            var getProductDtoRequest = new GetProductByIdRequestDto
            {
                ProductId = id
            };

            var validationResult = _getProductByIdRequestDtoValidator.Validate(getProductDtoRequest);

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