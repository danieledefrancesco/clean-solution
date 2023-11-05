using System;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Products;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestEase;
using TechTalk.SpecFlow;

namespace AspNetCore.Examples.ProductService.Specs.Steps
{
    [Binding]
    [Scope(Feature = "UpdateProduct")]
    public sealed class UpdateProductStepDefinitions(ScenarioContext scenarioContext)
    {
        private readonly ScenarioContext _scenarioContext = scenarioContext;

        [Given(@"an update product request <(.*), (.*)>")]
        public void GivenAnUpdateProductRequest(string productName, decimal productPrice)
        {
            TestData.UpdateProductRequest = new UpdateProductCommandRequestDtoBody
            {
                Name = productName,
                Price = productPrice
            };
        }

        [When(@"I make a PATCH request to the \/products\/(.+) endpoint")]
        public async Task WhenIMakeApatchRequestToTheProductsEndpoint(string productId)
        {
            try
            {
                TestData.ProductResponse = await Services.ProductServiceClient.Update(productId, TestData.UpdateProductRequest);
            }
            catch (ApiException e)
            {
                Console.WriteLine(">>>>"+e.Content);
                TestData.ApiError = e;
            }
        }

        [Then(@"the product (.+) has been successfully updated in the database")]
        public async Task ThenTheProductHasBeenSuccessfullyUpdatedInTheDatabase(string productId)
        {
            Services.AppDbContext.ChangeTracker.Clear();
            ProductId pId = ProductId.From(productId);
            var product = await Services.AppDbContext.Products.FirstOrDefaultAsync(x => x.Id == pId);
            product.Should().NotBeNull();
            product!.Name.Value.Should().Be(TestData.UpdateProductRequest.Name);
            product!.Price.Value.Should().Be(TestData.UpdateProductRequest.Price);
        }

        [Then(@"an OnProductUpdatedEvent is created in the queue for product (.+)")]
        public async Task ThenTheOnProductUpdatedEventIsCreatedInTheQueue(string productId)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            var message = await Services.OnProductUpdatedEventQueueClient.ReceiveMessageAsync();
            var @event = JsonConvert.DeserializeObject<OnProductUpdatedEventDto>(message.Value.Body.ToString());
            @event!.ProductId.Should().Be(productId);
            @event!.ProductName.Should().Be(TestData.UpdateProductRequest.Name);
            @event!.ProductPrice.Should().Be(TestData.UpdateProductRequest.Price);
        }
    }
}