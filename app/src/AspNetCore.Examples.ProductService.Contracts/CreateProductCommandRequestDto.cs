namespace AspNetCore.Examples.ProductService
{
    public sealed class CreateProductCommandRequestDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}