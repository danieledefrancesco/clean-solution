using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public sealed class AlreadyExistsErrorHandler : ErrorHandlerBase<AlreadyExistsError>
    {
        private readonly IMapper _mapper;

        public AlreadyExistsErrorHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public override IActionResult HandleError(IError error)
        {
            return new ObjectResult(_mapper.Map<ErrorDto>(error))
            {
                StatusCode = 409
            };
        }
    }
}