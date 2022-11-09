using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCore.Examples.ProductService.GetProductWithPriceCardById
{
    public sealed class GetProductWithPriceCardByIdRequestDtoModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) return Task.CompletedTask;
            var id = bindingContext.ValueProvider.GetValue("id");
            var request = new GetProductWithPriceCardByIdRequestDto
            {
                ProductId = id.FirstValue
            };
            bindingContext.Result = ModelBindingResult.Success(request);
            return Task.CompletedTask;
        }
    }
}