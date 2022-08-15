using System.Threading.Tasks;
using AspNetCore.Examples.PriceCardService;
using FluentAssertions;
using RestEase;
using TechTalk.SpecFlow;

namespace AspNetCore.Examples.ProductService.Specs.Steps
{
    [Binding]
    public sealed class GetProductStepDefinitions
    {

        private readonly ScenarioContext _scenarioContext;
        


        [When(@"I make a GET request to the /products/(.+) endpoint")]
        public async Task WhenIMakeAGetRequestToTheProductsIdEndpoint(string productId)
        {
            try
            {
                TestData.ProductResponse = await Services.ProductServiceClient.GetById(productId);
            }
            catch (RestEase.ApiException e)
            {
                TestData.ApiError = e;
            }
        }

        [Then(@"the product id is (.+)")]
        public void ThenTheProductIdIsProduct_Id(string productId)
        {
            TestData.ProductResponse.Should().NotBeNull();
            TestData.ProductResponse.GetContent().Id.Should().Be(productId);
        }

        [Then(@"the product name is (.+)")]
        public void ThenTheProductNameIsProduct_Name(string productName)
        {
            TestData.ProductResponse.Should().NotBeNull();
            TestData.ProductResponse.GetContent().Name.Should().Be(productName);
        }

        [Then(@"the product price is (.*)")]
        public void ThenTheProductPriceIs(decimal productPrice)
        {
            TestData.ProductResponse.Should().NotBeNull();
            TestData.ProductResponse.GetContent().Price.Should().Be(productPrice);
        }
        
        [Then(@"the product final price is (.*)")]
        public void ThenTheProductFinalPriceIs(decimal productFinalPrice)
        {
            TestData.ProductResponse.Should().NotBeNull();
            TestData.ProductResponse.GetContent().FinalPrice.Should().Be(productFinalPrice);
        }
    }
}