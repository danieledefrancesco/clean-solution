using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Specs.Wiremock;
using RestEase;

namespace AspNetCore.Examples.ProductService.Specs
{
    [BasePath("/__admin")]
    public interface IWiremockAdminClient
    {
        [Delete("mappings")]
        public Task DeleteMappings();

        [Post("mappings/reset")]
        public Task ResetMappings();

        [Post("mappings")]
        public Task CreateMapping([Body] Mapping mapping);

        [Post("scenarios/reset")]
        public Task ResetScenarios();

    }
}