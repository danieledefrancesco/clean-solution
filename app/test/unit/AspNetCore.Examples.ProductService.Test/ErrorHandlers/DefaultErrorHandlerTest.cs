using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public sealed class DefaultErrorHandlerTest: ErrorHandlerTestBase<DefaultErrorHandler>
    {
        private IMapper _mapper;
        protected override DefaultErrorHandler ErrorHandler { get; set; }
        protected override int ExpectedStatusCode => 400;
        
        [SetUp]
        public void SetUp()
        {
            _mapper = Substitute.For<IMapper>();
            ErrorHandler = new DefaultErrorHandler(_mapper);
            var error = new ErrorDto
            {
                Message = string.Empty
            };
            _mapper.Map<ErrorDto>(Arg.Any<IError>()).Returns(error);
        }

        [Test]
        public void Supports_ReturnsTrue()
        {
            ErrorHandler
                .Supports(Substitute.For<IError>())
                .Should()
                .BeTrue();
        }
        
        protected override IError CreateErrorInstance()
        {
            return Substitute.For<IError>();
        }
    }
}