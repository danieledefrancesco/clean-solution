namespace AspNetCore.Examples.ProductService.Specs.Wiremock
{
    public sealed class Mapping
    {
        public string Id { get; set; }
        public Request Request { get; set; }
        public Response Response { get; set; }
        public int Priority { get; set; } = 1;
    }
}