using System.Linq;
using AspNetCore.Examples.ProductService.Events;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Products
{
    public class ProductsFactoryTest
    {
        private ProductsFactory _productsFactory;

        [SetUp]
        public void SetUp()
        {
            _productsFactory = new ProductsFactory();
        }

        [Test]
        public void CreateProduct_ShouldReturnNewProduct()
        {
            const string productId = "id";
            const string productName = "name";
            const decimal productPrice = 1;
            var product = _productsFactory.CreateProduct(
                ProductId.From(productId),
                ProductName.From(productName),
                ProductPrice.From(productPrice));

            product.Id.Value.Should().Be(productId);
            product.Name.Value.Should().Be(productName);
            product.Price.Value.Should().Be(productPrice);
            product.DomainEvents.Where(domainEvent => domainEvent is OnProductCreated).Should().HaveCount(1);
        }
    }
}