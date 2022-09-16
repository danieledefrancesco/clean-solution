using AspNetCore.Examples.ProductService.Requests;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Validators
{
    public class GetProductDtoRequestValidatorTest
    {
        private GetProductDtoRequestValidator _getProductDtoRequestValidator;

        [SetUp]
        public void SetUp()
        {
            _getProductDtoRequestValidator = new GetProductDtoRequestValidator();
        }

        [TestCase("a1")]
        [TestCase("a2")]
        [TestCase("A1")]
        [TestCase("A-902_ sb")]
        public void GetProductDTORequestValidator_ThrowsNoError_IfProductIdIsValid(string id)
        {
            var getProductDtoRequest = new GetProductRequestDto()
            {
                ProductId = id
            };

            var validationResult = _getProductDtoRequestValidator.Validate(getProductDtoRequest);

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
            var getProductDtoRequest = new GetProductRequestDto()
            {
                ProductId = id
            };

            var validationResult = _getProductDtoRequestValidator.Validate(getProductDtoRequest);

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