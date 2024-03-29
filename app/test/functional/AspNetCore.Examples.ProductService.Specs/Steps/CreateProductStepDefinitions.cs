using System;
using System.Collections.Generic;
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
    public sealed class CreateProductStepDefinitions(ScenarioContext scenarioContext)
    {
        private readonly ScenarioContext _scenarioContext = scenarioContext;

        [Given(@"a create product request <(.*), (.*), (.*)>")]
        public void GivenACreateProductRequest(string productId, string productName, decimal productPrice)
        {
            TestData.CreateProductRequest = new CreateProductCommandRequestDto
            {
                Id = productId,
                Name = productName,
                Price = productPrice
            };
        }

        [When(@"I make a (.*) request to the \/products endpoint")]
        public async Task WhenIMakeApostRequestToTheProductsEndpoint(string method)
        {
            IDictionary<string, Func<CreateProductCommandRequestDto, Task<Response<ProductDto>>>> request =
                new Dictionary<string, Func<CreateProductCommandRequestDto, Task<Response<ProductDto>>>>
                {
                    { "POST", Services.ProductServiceClient.CreatePost },
                    { "PUT", Services.ProductServiceClient.CreatePut }
                };
            try
            {
                TestData.ProductResponse = await request[method].Invoke(TestData.CreateProductRequest);
            }
            catch (ApiException e)
            {
                Console.WriteLine(">>>>"+e.Content);
                TestData.ApiError = e;
            }
        }

        [Then(@"the product has been successfully created in the database")]
        public async Task ThenTheProductHasBeenSuccessfullyCreatedInTheDatabase()
        {
            ProductId productId = ProductId.From(TestData.CreateProductRequest.Id);
            var product = await Services.AppDbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            product.Should().NotBeNull();
            product!.Name.Value.Should().Be(TestData.CreateProductRequest.Name);
            product!.Price.Value.Should().Be(TestData.CreateProductRequest.Price);
        }

        [Then(@"an OnProductCreatedEvent is created in the queue")]
        public async Task ThenTheOnProductCreatedEventIsCreatedInTheQueue()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            var message = await Services.OnProductCreatedEventQueueClient.ReceiveMessageAsync();
            var @event = JsonConvert.DeserializeObject<OnProductCreatedEventDto>(message.Value.Body.ToString());
            @event!.ProductId.Should().Be(TestData.CreateProductRequest.Id);
            @event!.ProductName.Should().Be(TestData.CreateProductRequest.Name);
            @event!.ProductPrice.Should().Be(TestData.CreateProductRequest.Price);
        }
    }
}