using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.DataTransferObjects
{
    public sealed class GetProductByIdRequestDto
    {
        [FromRoute(Name = "id")]
        public string ProductId { get; set; }
    }
}