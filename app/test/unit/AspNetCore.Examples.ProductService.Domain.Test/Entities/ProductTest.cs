using AspNetCore.Examples.ProductService.ValueObjects;

namespace AspNetCore.Examples.ProductService.Entities
{
    public sealed class ProductTest : EntityTestBase<Product, ProductId>
    {
        protected override ProductId CreateId()
        {
            return ProductId.From("id");
        }

        protected override ProductId CreateDifferentId()
        {
            return ProductId.From("another_id");
        }

        protected override Product CreateEntityById(ProductId id)
        {
            return new Product(id);
        }
    }
}