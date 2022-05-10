using AspNetCore.Examples.ProductService.Errors;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class PriceCardNewPriceLessThanZeroErrorHandler : IErrorHandler
    {
        public bool Supports(IError error)
        {
            return error.GetType() == typeof(PriceCardNewPriceLessThanZeroError);
        }

        public IActionResult HandleError(IError error)
        {
            return new ObjectResult(error)
            {
                StatusCode = 422
            };
        }
    }
}