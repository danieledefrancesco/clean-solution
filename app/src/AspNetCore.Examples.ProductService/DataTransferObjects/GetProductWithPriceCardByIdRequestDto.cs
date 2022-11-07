using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.DataTransferObjects
{
    public sealed class GetProductWithPriceCardByIdRequestDto
    {
        [FromRoute(Name = "id")]
        public string ProductId { get; set; }
    }
}