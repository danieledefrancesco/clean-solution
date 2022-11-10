using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCore.Examples.ProductService.GetProductWithPriceCardById
{
    public sealed class GetProductWithPriceCardByIdRequestDtoModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context?.Metadata.ModelType == typeof(GetProductWithPriceCardByIdRequestDto) ? new GetProductWithPriceCardByIdRequestDtoModelBinder() : null;
        }
    }
}