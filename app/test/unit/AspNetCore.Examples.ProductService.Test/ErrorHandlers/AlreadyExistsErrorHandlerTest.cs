using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public sealed class AlreadyExistsErrorHandlerTest : ErrorHandlerTestBase<AlreadyExistsErrorHandler, AlreadyExistsError>
    {
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = Substitute.For<IMapper>();
            ErrorHandler = new AlreadyExistsErrorHandler(_mapper);
            var error = new ErrorDto
            {
                Message = string.Empty
            };
            _mapper.Map<ErrorDto>(Arg.Any<IError>()).Returns(error);
        }

        protected override AlreadyExistsErrorHandler ErrorHandler { get; set; }

        protected override int ExpectedStatusCode => 409;

        protected override IError CreateErrorInstance()
        {
            return new AlreadyExistsError();
        }

        
    }
}