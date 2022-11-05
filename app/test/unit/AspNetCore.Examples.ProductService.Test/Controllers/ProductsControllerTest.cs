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
    public sealed class ProductsControllerTest
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

            var product = new Product(ProductId.From(productId))
            {
                Price = ProductPrice.From(productPrice),
                Name = ProductName.From(productName)
            };

            var getProductByIdResponse = new GetProductWithPriceCardByIdResponse(new ProductWithPriceCard(product, null));

            _mediator.Send(Arg.Any<GetProductWithPriceCardByIdRequest>(), Arg.Any<CancellationToken>()).Returns(getProductByIdResponse);

            var productDto = new ProductDto()
            {
                Id = productId,
                Name = product.Name.Value,
                Price = product.Price.Value
            };


            var getProductDtoRequest = new GetProductWithPriceCardByIdRequestDto
            {
                ProductId = productId
            };
            
            _mapper.Map<ProductDto>(getProductByIdResponse).Returns(productDto);

            
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

            var createProductRequestDto = new CreateProductRequestDto
            {
                Id = productId,
                Price = productPrice,
                Name = productName
            };

            var createProductRequest = new CreateProductCommandRequest(ProductId.From(productId),
                ProductName.From(productName), ProductPrice.From(productPrice));
            
            var product = new Product(ProductId.From(productId))
            {
                Name = ProductName.From(createProductRequestDto.Name),
                Price = ProductPrice.From(createProductRequestDto.Price)
            };
            
            var productDto = new ProductDto
            {
                Id = productId,
                Name = ProductName.From(createProductRequestDto.Name),
                Price = ProductPrice.From(createProductRequestDto.Price)
            };

            var createProductResponse = new CreateProductCommandResponse(product);

            _mediator.Send(Arg.Any<CreateProductCommandRequest>(), Arg.Any<CancellationToken>()).Returns(createProductResponse);
            
            _mapper.Map<ProductDto>(createProductResponse).Returns(productDto);
            _mapper.Map<CreateProductCommandRequest>(createProductRequestDto).Returns(createProductRequest);
            
            var actionResult = _productsController.Insert(createProductRequestDto).Result;

            actionResult.Should().BeOfType<OkObjectResult>();
            
            var objectResult = actionResult as OkObjectResult;
            objectResult!.StatusCode.Should().Be(200);
            objectResult!.Value.Should().Be(productDto);
        }
    }
}