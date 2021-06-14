namespace AspNetCore.Examples.ProductService.Persistence.ClassMapRegistrationProviders
{
    public interface IMongoDbClassMapRegistrationProvider
    {
        void RegisterClassMap();
    }

    public interface IMongoDbClassMapRegistrationProvider<T> : IMongoDbClassMapRegistrationProvider
        where T : class
    {
        
    }
}