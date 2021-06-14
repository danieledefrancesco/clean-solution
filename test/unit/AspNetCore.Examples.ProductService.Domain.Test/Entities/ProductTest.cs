namespace AspNetCore.Examples.ProductService.Entities
{
    public class ProductTest : EntityTestBase<Product, string>
    {
        protected override string CreateId()
        {
            return "id";
        }

        protected override string CreateDifferentId()
        {
            return "another_id";
        }

        protected override Product CreateEntityById(string id)
        {
            return new Product()
            {
                Id = id
            };
        }
    }
}