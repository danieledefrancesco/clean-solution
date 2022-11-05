using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.Requests
{
    public sealed class GetProductWithPriceCardByIdRequestDto
    {
        [FromRoute(Name = "id")]
        public string ProductId { get; set; }
    }
}