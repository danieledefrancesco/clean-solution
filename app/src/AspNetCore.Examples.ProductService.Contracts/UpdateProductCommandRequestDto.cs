namespace AspNetCore.Examples.ProductService
{
    public sealed class UpdateProductCommandRequestDto
    {
        public string Id { get; set; }
        public UpdateProductCommandRequestDtoBody Body { get; set; }
    }
}