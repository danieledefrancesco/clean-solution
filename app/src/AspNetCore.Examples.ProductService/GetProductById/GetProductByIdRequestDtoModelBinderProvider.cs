using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCore.Examples.ProductService.GetProductById
{
    public sealed class GetProductByIdRequestDtoModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context?.Metadata.ModelType == typeof(GetProductByIdRequestDto) ? new GetProductByIdRequestDtoModelBinder() : null;
        }
    }
}