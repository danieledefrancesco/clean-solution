using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.Requests
{
    public class GetProductRequestDto
    {
        public string ProductId { get; set; }

        public static ValueTask<GetProductRequestDto> BindAsync(HttpContext context)
            => ValueTask.FromResult(new GetProductRequestDto{
                ProductId = context.Request.RouteValues["id"]!.ToString()
                    });
    
    }
}