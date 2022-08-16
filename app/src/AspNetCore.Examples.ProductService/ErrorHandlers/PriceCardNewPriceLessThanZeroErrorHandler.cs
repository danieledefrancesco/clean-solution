using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class PriceCardNewPriceLessThanZeroErrorHandler : ErrorHandlerBase<PriceCardNewPriceLessThanZeroError>
    {
        private readonly IMapper _mapper;

        public PriceCardNewPriceLessThanZeroErrorHandler(IMapper mapper)
        {
            _mapper = mapper;
        }


        public override IActionResult HandleError(IError error)
        {
            return new ObjectResult(_mapper.Map<ErrorDto>(error))
            {
                StatusCode = 422
            };
        }
    }
}