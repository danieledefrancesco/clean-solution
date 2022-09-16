using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class NotFoundErrorHandler : ErrorHandlerBase<NotFoundError>
    {
        private readonly IMapper _mapper;

        public NotFoundErrorHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public override IResult HandleError(IError error) => Results.NotFound(_mapper.Map<ErrorDto>(error));
    }
}