using AutoMapper;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public abstract class ProfileTestBase<T> where T : Profile, new()
    {
        protected MapperConfiguration _mapperConfiguration;
        protected IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapperConfiguration = new MapperConfiguration(
                cfg => cfg.AddProfile<T>());
            _mapper = _mapperConfiguration.CreateMapper();
        }

        [Test]
        public void Configuration_IsValid()
        {
            _mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}