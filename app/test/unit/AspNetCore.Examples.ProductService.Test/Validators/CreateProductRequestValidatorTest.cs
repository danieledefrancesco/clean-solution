using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Validators
{
    public class CreateProductRequestValidatorTest
    {
        private CreateProductRequestValidator _createProductRequestValidator;

        [SetUp]
        public void SetUp()
        {
            _createProductRequestValidator = new CreateProductRequestValidator();
        }

        [TestCase("id","Name")]
        [TestCase("id 1"," name")]
        [TestCase("id-1","Name")]
        [TestCase("id_1","Name123#")]
        [TestCase("ID1","Name_- @#!2")]
        public void ProductValidator_ThrowsNoError_IfProductIdAndNameAreValid(string productId, string productName)
        {
            var createProductRequest = new CreateProductRequestDto()
            {
                Id = productId,
                Name = productName
            };
            
            var validationResult = _createProductRequestValidator.Validate(createProductRequest);

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
        [TestCase("i.d")]
        [TestCase("_id")]
        [TestCase(" ")]
        [TestCase("-id")]
        [TestCase("1id")]
        [TestCase(" id")]
        [TestCase("_id")]
        public void ProductValidator_ThrowsError_IfProductIdIsInvalid(string productId)
        {
            var createProductRequest = new CreateProductRequestDto()
            {
                Id = productId,
                Name = "productName"
            };
            
            var validationResult = _createProductRequestValidator.Validate(createProductRequest);

            validationResult
                .Should()
                .NotBeNull();

            var errors = validationResult.Errors;
            errors
                .Should()
                .NotBeEmpty();
        }
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ProductValidator_ThrowsError_IfProductNameIsInvalid(string productName)
        {
            var createProductRequest = new CreateProductRequestDto()
            {
                Id = "id",
                Name = productName
            };
            
            var validationResult = _createProductRequestValidator.Validate(createProductRequest);

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