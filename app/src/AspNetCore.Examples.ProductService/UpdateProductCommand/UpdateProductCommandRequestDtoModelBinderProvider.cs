using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCore.Examples.ProductService.UpdateProductCommand
{
    public sealed class UpdateProductCommandRequestDtoModelBinderProvider: IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context?.Metadata.ModelType == typeof(UpdateProductCommandRequestDto) ? new UpdateProductCommandRequestDtoModelBinder() : null;
        }
    }
}