using System;
using System.Linq;

namespace AspNetCore.Examples.ProductService.Persistence.ClassMapRegistrationProviders
{
    public class MongoDbClassMapsRegister : IMongoDbClassMapsRegister
    {
        public void RegisterAllClassMaps()
        {
            typeof(MongoDbClassMapsRegister)
                .Assembly
                .ExportedTypes
                .Where(IsConcreteMongoDbClassMapRegistrationProvider)
                .Select(CreateInstanceForIMongoDbClassMapRegistrationProvider)
                .ToList()
                .ForEach(registrationProvider => registrationProvider.RegisterClassMap());
        }

        private IMongoDbClassMapRegistrationProvider CreateInstanceForIMongoDbClassMapRegistrationProvider(Type type)
        {
            return Activator.CreateInstance(type) as IMongoDbClassMapRegistrationProvider;
        }

        private bool IsConcreteMongoDbClassMapRegistrationProvider(Type type)
        {
            return !type.IsAbstract && !type.IsInterface && typeof(IMongoDbClassMapRegistrationProvider).IsAssignableFrom(type);
        }
    }
}