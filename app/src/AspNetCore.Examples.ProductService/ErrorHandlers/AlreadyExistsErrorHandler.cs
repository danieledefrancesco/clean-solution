using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class AlreadyExistsErrorHandler : ErrorHandlerBase<AlreadyExistsError>
    {
        private readonly IMapper _mapper;

        public AlreadyExistsErrorHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public override IResult HandleError(IError error) => Results.Conflict(_mapper.Map<ErrorDto>(error));
    }
}