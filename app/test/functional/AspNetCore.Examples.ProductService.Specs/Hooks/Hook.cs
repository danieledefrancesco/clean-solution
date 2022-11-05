using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace AspNetCore.Examples.ProductService.Specs.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        [BeforeScenario]
        public async Task ClearDatabase()
        {
            Services.AppDbContext.Products.RemoveRange(Services.AppDbContext.Products.ToList());
            await Services.AppDbContext.SaveChangesAsync();
        }
        
        [BeforeScenario]
        public void ResetTestData()
        {
            TestData.Reset();
        }
        
        [BeforeScenario]
        public void ResetServices()
        {
            Services.Reset();
        }

        [BeforeScenario]
        public async Task ResetWiremock()
        {
            await Services.WiremockAdminClient.ResetMappings();
            await Services.WiremockAdminClient.ResetScenarios();
        }
    }
}