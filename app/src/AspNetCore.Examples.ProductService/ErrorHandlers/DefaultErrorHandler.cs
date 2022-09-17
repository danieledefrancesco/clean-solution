using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class DefaultErrorHandler : ErrorHandlerBase
    {
        private readonly IMapper _mapper;

        public DefaultErrorHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public override bool Supports(IError error)
        {
            return true;
        }

        public override IResult HandleError(IError error) => Results.BadRequest(_mapper.Map<ErrorDto>(error));
    }
}