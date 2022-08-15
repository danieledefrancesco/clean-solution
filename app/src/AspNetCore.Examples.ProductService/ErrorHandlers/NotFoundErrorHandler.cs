using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class NotFoundErrorHandler : ErrorHandlerBase<NotFoundError>
    {
        private readonly IMapper _mapper;

        public NotFoundErrorHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public override IActionResult HandleError(IError error)
        {
            return new NotFoundObjectResult(_mapper.Map<ErrorDto>(error));
        }
    }
}