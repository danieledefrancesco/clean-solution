using AspNetCore.Examples.ProductService.Entities;

namespace AspNetCore.Examples.ProductService.Products
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