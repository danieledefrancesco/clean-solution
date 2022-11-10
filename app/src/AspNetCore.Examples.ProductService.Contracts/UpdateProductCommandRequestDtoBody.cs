namespace AspNetCore.Examples.ProductService
{
    public sealed class UpdateProductCommandRequestDtoBody
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}