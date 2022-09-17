using AspNetCore.Examples.ProductService.Errors;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public abstract class ErrorHandlerBase : IErrorHandler
    {
        public abstract bool Supports(IError error);
        public abstract IResult HandleError(IError error);
    }

    public abstract class ErrorHandlerBase<T> : ErrorHandlerBase where T : IError
    {
        public override bool Supports(IError error)
        {
            return error.GetType() == typeof(T);
        }
    }
}