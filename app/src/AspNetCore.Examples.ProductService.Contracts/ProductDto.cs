namespace AspNetCore.Examples.ProductService
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
    }
}