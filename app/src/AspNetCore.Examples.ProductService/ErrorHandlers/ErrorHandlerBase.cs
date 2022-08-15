using AspNetCore.Examples.ProductService.Errors;
using AutoMapper;
using Elasticsearch.Net.Specification.IndexLifecycleManagementApi;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public abstract class ErrorHandlerBase : IErrorHandler
    {
        public abstract bool Supports(IError error);
        public abstract IActionResult HandleError(IError error);
    }

    public abstract class ErrorHandlerBase<T> : ErrorHandlerBase where T : IError
    {
        public override bool Supports(IError error)
        {
            return error.GetType() == typeof(T);
        }
    }
}