using AspNetCore.Examples.ProductService.DataTransferObjects;
using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public sealed class PriceCardNewPriceLessThanZeroErrorHandlerTest: ErrorHandlerTestBase<PriceCardNewPriceLessThanZeroErrorHandler, PriceCardNewPriceLessThanZeroError>
    {
        private IMapper _mapper;
        protected override PriceCardNewPriceLessThanZeroErrorHandler ErrorHandler { get; set; }
        protected override int ExpectedStatusCode => 422;

        [SetUp]
        public void SetUp()
        {
            _mapper = Substitute.For<IMapper>();
            ErrorHandler = new PriceCardNewPriceLessThanZeroErrorHandler(_mapper);
            var error = new ErrorDto
            {
                Message = string.Empty
            };
            _mapper.Map<ErrorDto>(Arg.Any<IError>()).Returns(error);
        }

        protected override IError CreateErrorInstance()
        {
            return new PriceCardNewPriceLessThanZeroError();
        }
    }
}