using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.UpdateProductCommand
{
    public sealed class UpdateProductCommandRequestDtoValidatorTest
    {
        private UpdateProductCommandRequestDtoValidator _updateProductRequestDtoValidator;

        [SetUp]
        public void SetUp()
        {
            _updateProductRequestDtoValidator = new UpdateProductCommandRequestDtoValidator();
        }

        [TestCase("id","Name")]
        [TestCase("id 1"," name")]
        [TestCase("id-1","Name")]
        [TestCase("id_1","Name123#")]
        [TestCase("ID1","Name_- @#!2")]
        public void ProductValidator_ThrowsNoError_IfProductIdAndNameAreValid(string productId, string productName)
        {
            var updateProductRequest = new UpdateProductCommandRequestDto
            {
                Id = productId,
                Body = new UpdateProductCommandRequestDtoBody
                {
                    Name = productName,
                    Price = 1
                }
            };
            
            var validationResult = _updateProductRequestDtoValidator.Validate(updateProductRequest);

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
            var updateProductCommandRequest = new UpdateProductCommandRequestDto
            {
                Id = productId,
                Body = new UpdateProductCommandRequestDtoBody
                {
                    Name = "productName",
                    Price = 1
                }
            };
            
            var validationResult = _updateProductRequestDtoValidator.Validate(updateProductCommandRequest);

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
            var updateProductRequest = new UpdateProductCommandRequestDto
            {
                Id = "id",
                Body = new UpdateProductCommandRequestDtoBody
                {
                    Name = productName,
                    Price = 1
                }
            };
            
            var validationResult = _updateProductRequestDtoValidator.Validate(updateProductRequest);

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