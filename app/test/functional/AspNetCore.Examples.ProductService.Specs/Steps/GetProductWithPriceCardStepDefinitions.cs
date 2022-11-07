using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AspNetCore.Examples.ProductService.Specs.Steps
{
    [Binding]
    [Scope(Feature = "GetProductWithPriceCard")]
    public sealed class GetProductWithPriceCardStepDefinitions
    {

        [When(@"I make a GET request to the \/products\/(.+)\/with-price-card endpoint")]
        public async Task WhenIMakeAGetRequestToTheProductsIdWithPriceCardEndpoint(string productId)
        {
            try
            {
                TestData.ProductWithPriceCardResponse = await Services.ProductServiceClient.GetWithPriceCardById(productId);
            }
            catch (RestEase.ApiException e)
            {
                TestData.ApiError = e;
            }
        }

        [Then(@"the product with price card id is (.+)")]
        public void ThenTheProductWithPriceCardIdIsProduct_Id(string productId)
        {
            TestData.ProductWithPriceCardResponse.Should().NotBeNull();
            TestData.ProductWithPriceCardResponse.GetContent().Id.Should().Be(productId);
        }

        [Then(@"the product with price card name is (.+)")]
        public void ThenTheProductWithPriceCardNameIsProduct_Name(string productName)
        {
            TestData.ProductWithPriceCardResponse.Should().NotBeNull();
            TestData.ProductWithPriceCardResponse.GetContent().Name.Should().Be(productName);
        }

        [Then(@"the product with price card price is (.*)")]
        public void ThenTheProductWithPriceCardPriceIs(decimal productPrice)
        {
            TestData.ProductWithPriceCardResponse.Should().NotBeNull();
            TestData.ProductWithPriceCardResponse.GetContent().Price.Should().Be(productPrice);
        }
        
        [Then(@"the product with price card final price is (.*)")]
        public void ThenTheProductFinalPriceIs(decimal productFinalPrice)
        {
            TestData.ProductWithPriceCardResponse.Should().NotBeNull();
            TestData.ProductWithPriceCardResponse.GetContent().FinalPrice.Should().Be(productFinalPrice);
        }

        [Then(@"the product has a price card")]
        public void ThenTheProductHasAPriceCard()
        {
            TestData.ProductWithPriceCardResponse.Should().NotBeNull();
            var productWithPriceCard = TestData.ProductWithPriceCardResponse.GetContent();
            productWithPriceCard.Should().NotBeNull();
            productWithPriceCard.PriceCard.Should().NotBeNull();
            productWithPriceCard.FinalPrice.Should().Be(productWithPriceCard.PriceCard.NewPrice);
        }
        
        
        [Then(@"the product doesn't have a price card")]
        public void ThenTheProductDoesntHaveAPriceCard()
        {
            TestData.ProductWithPriceCardResponse.Should().NotBeNull();
            var productWithPriceCard = TestData.ProductWithPriceCardResponse.GetContent();
            productWithPriceCard.Should().NotBeNull();
            productWithPriceCard.PriceCard.Should().BeNull();
            productWithPriceCard.FinalPrice.Should().Be(productWithPriceCard.Price);
        }
    }
}