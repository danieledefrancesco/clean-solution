using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCore.Examples.PriceCardService;
using AspNetCore.Examples.ProductService.Products;
using AspNetCore.Examples.ProductService.Specs.Wiremock;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AspNetCore.Examples.ProductService.Specs.Steps;

[Binding]
public sealed class GenericProductsStepDefinitions(ScenarioContext scenarioContext)
{
    private readonly ScenarioContext _scenarioContext = scenarioContext;

    [Given("a product <(.+), (.+), (.*)>")]
    public async Task GivenAProduct(string id, string name, decimal price)
    {
        await Services.AppDbContext.Products.AddAsync(new Product(ProductId.From(id), ProductName.From(name), ProductPrice.From(price)));
        await Services.AppDbContext.SaveChangesAsync();
    }

    [Then(@"the response status code is (.*)")]
    public void ThenTheResponseStatusCodeIs(int statusCode)
    {
        TestData.HttpStatusCode.Should().Be(statusCode);
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