using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework.Internal;
using TechTalk.SpecFlow;

namespace AspNetCore.Examples.ProductService.Specs.Steps
{
    [Binding]
    [Scope(Feature = "GetProduct")]

    public sealed class GetProductStepDefinitions
    {
        [When(@"I make a GET request to the \/products\/(.+) endpoint")]
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
    }
}