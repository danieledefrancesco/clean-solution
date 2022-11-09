using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCore.Examples.ProductService.GetProductById
{
    public sealed class GetProductByIdRequestDtoModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) return Task.CompletedTask;
            var id = bindingContext.ValueProvider.GetValue("id");
            var request = new GetProductByIdRequestDto
            {
                ProductId = id.FirstValue
            };
            bindingContext.Result = ModelBindingResult.Success(request);
            return Task.CompletedTask;
        }
    }
}