using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCore.Examples.ProductService.UpdateProductCommand
{
    public sealed class UpdateProductCommandRequestDtoModelBinder: IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) return;
            var id = bindingContext.ValueProvider.GetValue("id");
            var bodyString = await new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEndAsync();
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var body = JsonSerializer.Deserialize<UpdateProductCommandRequestDtoBody>(bodyString, serializeOptions);
            var model = new UpdateProductCommandRequestDto
            {
                Id = id.FirstValue,
                Body = body
            };
            bindingContext.Result = ModelBindingResult.Success(model);
        }
    }
}