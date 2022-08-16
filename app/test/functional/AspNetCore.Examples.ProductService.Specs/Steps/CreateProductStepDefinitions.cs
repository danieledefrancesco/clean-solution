using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RestEase;
using TechTalk.SpecFlow;

namespace AspNetCore.Examples.ProductService.Specs.Steps
{
    [Binding]
    public class CreateProductStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public CreateProductStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"a create product request <(.*), (.*), (.*)>")]
        public void GivenACreateProductRequest(string productId, string productName, decimal productPrice)
        {
            TestData.CreateProductRequest = new CreateProductRequestDto
            {
                Id = productId,
                Name = productName,
                Price = productPrice
            };
        }

        [When(@"I make a (.*) request to the /products endpoint")]
        public async Task WhenIMakeApostRequestToTheProductsEndpoint(string method)
        {
            IDictionary<string, Func<CreateProductRequestDto, Task<Response<ProductDto>>>> request =
                new Dictionary<string, Func<CreateProductRequestDto, Task<Response<ProductDto>>>>
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
            var product = await Services.AppDbContext.Products.FirstOrDefaultAsync(x => x.Id == TestData.CreateProductRequest.Id);
            product.Should().NotBeNull();
            product!.Name.Value.Should().Be(TestData.CreateProductRequest.Name);
            product!.Price.Value.Should().Be(TestData.CreateProductRequest.Price);
        }
    }
}