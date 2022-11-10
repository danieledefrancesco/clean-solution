namespace AspNetCore.Examples.ProductService.Products
{
    public interface IProductsFactory
    {
        Product CreateProduct(ProductId productId, ProductName productName, ProductPrice productPrice);
    }
}