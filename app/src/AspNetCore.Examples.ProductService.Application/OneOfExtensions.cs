using System;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Errors;
using OneOf;

namespace AspNetCore.Examples.ProductService
{
    public static class OneOfExtensions
    {
        public static Task<OneOf<T1, ErrorBase>> ThrowErrorOrContinueWith<T1, T2>(this OneOf<T2, ErrorBase> oneOf,
            Func<T2, Task<OneOf<T1, ErrorBase>>> continueWith)
        {
            return oneOf.Match(
                async t2 => await continueWith(t2), 
                error => Task.FromResult((OneOf<T1,ErrorBase>)error));
        }
        public static Task<OneOf<T1, ErrorBase>> ThrowErrorOrContinueWith<T1, T2>(this OneOf<T2, ErrorBase> oneOf,
            Func<T2, Task<T1>> continueWith)
        {
            return oneOf.Match(
                async t2 => (OneOf<T1, ErrorBase>) await continueWith(t2), 
                error => Task.FromResult((OneOf<T1,ErrorBase>)error));
        }
    }
}