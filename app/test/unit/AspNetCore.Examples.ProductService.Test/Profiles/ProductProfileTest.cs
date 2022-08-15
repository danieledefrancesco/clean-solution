using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.ValueObjects;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public class ProductProfileTest : ProfileTestBase<ProductProfile>
    {
        [Test]
        public void Product_ToProductDto_IsMappedCorrectly()
        {
            var product = new Product()
            {
                Id = "Id",
                Name = ProductName.From("Name")
            };

            var productDto = _mapper.Map<ProductDto>(product);

            productDto.Id
                .Should()
                .Be(product.Id);
            
            productDto.Name
                .Should()
                .Be(product.Name.Value);
        }
        
        [Test]
        public void CreateProductRequestDto_ToProduct_IsMappedCorrectly()
        {
            var createProductRequestDto = new CreateProductRequestDto
            {
                Id = "Id",
                Name = "Name"
            };

            var product = _mapper.Map<Product>(createProductRequestDto);

            product.Id
                .Should()
                .Be(createProductRequestDto.Id);
            
            product.Name
                .Should()
                .Be(ProductName.From(createProductRequestDto.Name));
        }

    }
}