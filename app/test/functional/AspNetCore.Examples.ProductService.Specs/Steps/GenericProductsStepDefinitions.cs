using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCore.Examples.PriceCardService;
using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Specs.Wiremock;
using AspNetCore.Examples.ProductService.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal;
using TechTalk.SpecFlow;
using ApiException = RestEase.ApiException;

namespace AspNetCore.Examples.ProductService.Specs.Steps
{
    [Binding]
    public class GenericProductsStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public GenericProductsStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("a product <(.+), (.+), (.*)>")]
        public async Task GivenAProduct(string id, string name, decimal price)
        {
            await Services.AppDbContext.Products.AddAsync(new Product
            {
                Id = id,
                Name = ProductName.From(name),
                Price = ProductPrice.From(price)
            });
            await Services.AppDbContext.SaveChangesAsync();
        }

        [Then(@"the response status code is (.*)")]
        public void ThenTheResponseStatusCodeIs(int statusCode)
        {
            TestData.HttpStatusCode.Should().Be(statusCode);
        }

        [Given(@"a price card <(.*), (.*), (.*), (.*), (.*), (.*)>")]
        public async Task GivenAPricecard(string priceCardId, string proudctId, string priceCardName, double priceCardPrice, DateTime validFrom, DateTime validUntil)
        {
            var priceCard = new PriceCard
            {
                Id = priceCardId,
                ProductId = proudctId,
                Name = priceCardName,
                NewPrice = priceCardPrice,
                ValidFrom = validFrom,
                ValidTo = validUntil
            };
            var priceCardList = new PriceCardList
            {
                Items = new List<PriceCard>()
                {
                    priceCard
                }
            };
            var mapping = new Mapping
            {
                Request = new Request
                {
                    Url = $"/price-card-service/price-cards/active/{proudctId}",
                    Method = "GET"
                },
                Response = new Response
                {
                    Body = Newtonsoft.Json.JsonConvert.SerializeObject(priceCardList),
                    Status = 200
                }
            };
            await Services.WiremockAdminClient.CreateMapping(mapping);
        }
    }
}