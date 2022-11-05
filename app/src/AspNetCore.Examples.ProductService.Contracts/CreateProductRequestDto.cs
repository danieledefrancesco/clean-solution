namespace AspNetCore.Examples.ProductService
{
    public sealed class CreateProductRequestDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}