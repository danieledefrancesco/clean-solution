using System;
using System.Threading;
using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using AspNetCore.Examples.ProductService.ValueObjects;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Controllers
{
    public class ProductsControllerTest
    {
        private ProductsController _productsController;
        private IErrorHandlerFactory _errorHandlerFactory;
        private IMediator _mediator;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _errorHandlerFactory = Substitute.For<IErrorHandlerFactory>();
            _mediator = Substitute.For<IMediator>();
            _mapper = Substitute.For<IMapper>();
            _productsController = new ProductsController(_errorHandlerFactory, _mediator, _mapper);
        }

        [Test]
        public void Get_ShouldReturnProduct_IfProductExists()
        {
            const string productId = "p1";
            const decimal productPrice = 1;
            const string productName = "name";

            var product = new Product
            {
                Id = productId,
                CreatedAt = DateTime.Now,
                LastModifiedAt = DateTime.Now,
                Price = ProductPrice.From(productPrice),
                Name = ProductName.From(productName)
            };

            var getProductByIdResponse = new GetProductByIdResponse(product);

            _mediator.Send(Arg.Any<GetProductByIdRequest>(), Arg.Any<CancellationToken>()).Returns(getProductByIdResponse);

            var productDto = new ProductDto()
            {
                Id = productId,
                Name = product.Name.Value,
                Price = product.Price.Value
            };

            _mapper.Map<ProductDto>(product).Returns(productDto);

            var getProductDtoRequest = new GetProductDtoRequest
            {
                ProductId = productId
            };
            
            var actionResult = _productsController.Get(getProductDtoRequest).Result;

            actionResult.Should().BeOfType<OkObjectResult>();
            
            var objectResult = actionResult as OkObjectResult;
            objectResult!.StatusCode.Should().Be(200);
            objectResult!.Value.Should().Be(productDto);
        }
        
        [Test]
        public void Insert_ShouldReturnProduct_IfProductDoesntExists()
        {
            const string productId = "p1";
            const decimal productPrice = 1;
            const string productName = "name";

            var productDto = new ProductDto
            {
                Id = productId,
                Price = productPrice,
                Name = productName
            };
            
            var product = new Product
            {
                Id = productId,
                Name = ProductName.From(productDto.Name),
                Price = ProductPrice.From(productDto.Price),
                CreatedAt = DateTime.Now,
                LastModifiedAt = DateTime.Now
            };

            var createProductResponse = new CreateProductCommandResponse(product);

            _mediator.Send(Arg.Any<CreateProductCommandRequest>(), Arg.Any<CancellationToken>()).Returns(createProductResponse);
            
            _mapper.Map<ProductDto>(product).Returns(productDto);
            _mapper.Map<Product>(productDto).Returns(product);
            
            var actionResult = _productsController.Insert(productDto).Result;

            actionResult.Should().BeOfType<OkObjectResult>();
            
            var objectResult = actionResult as OkObjectResult;
            objectResult!.StatusCode.Should().Be(200);
            objectResult!.Value.Should().Be(productDto);
        }
    }
}