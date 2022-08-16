using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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

        public override IActionResult HandleError(IError error)
        {
            return new ObjectResult(_mapper.Map<ErrorDto>(error))
            {
                StatusCode = 400
            };
        }
    }
}