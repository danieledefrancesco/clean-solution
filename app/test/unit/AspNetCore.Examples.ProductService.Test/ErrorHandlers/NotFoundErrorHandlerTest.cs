using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class NotFoundErrorHandlerTest: ErrorHandlerTestBase<NotFoundErrorHandler, NotFoundError>
    {
        private IMapper _mapper;
        protected override NotFoundErrorHandler ErrorHandler { get; set; }
        protected override int ExpectedStatusCode => 404;


        [SetUp]
        public void SetUp()
        {
            _mapper = Substitute.For<IMapper>();
            ErrorHandler = new NotFoundErrorHandler(_mapper);
            var error = new ErrorDto
            {
                Message = string.Empty
            };
            _mapper.Map<ErrorDto>(Arg.Any<IError>()).Returns(error);
        }

        protected override NotFoundError CreateErrorInstance()
        {
            return new NotFoundError();
        }
    }
}