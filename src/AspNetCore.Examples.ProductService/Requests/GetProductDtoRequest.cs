using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.Requests
{
    public class GetProductDtoRequest
    {
        [FromRoute(Name = "id")]
        public string ProductId { get; set; }
    }
}