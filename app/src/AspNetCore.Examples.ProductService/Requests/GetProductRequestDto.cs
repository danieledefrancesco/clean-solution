using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.Requests
{
    public class GetProductRequestDto
    {
        [FromRoute(Name = "id")]
        public string ProductId { get; set; }
    }
}