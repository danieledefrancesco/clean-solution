using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class PriceCardNewPriceLessThanZeroErrorHandler : ErrorHandlerBase<PriceCardNewPriceLessThanZeroError>
    {
        private readonly IMapper _mapper;

        public PriceCardNewPriceLessThanZeroErrorHandler(IMapper mapper)
        {
            _mapper = mapper;
        }


        public override IResult HandleError(IError error) =>
            Results.UnprocessableEntity(_mapper.Map<ErrorDto>(error));
    }
}