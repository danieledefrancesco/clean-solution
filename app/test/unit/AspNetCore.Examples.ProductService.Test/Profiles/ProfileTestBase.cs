using AutoMapper;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public abstract class ProfileTestBase<T> where T : Profile, new()
    {
        private MapperConfiguration _mapperConfiguration;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapperConfiguration = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<T>();
                    AddAdditionalConfig(cfg);
                });
            _mapper = _mapperConfiguration.CreateMapper();
        }

        protected virtual void AddAdditionalConfig(
            IMapperConfigurationExpression mapperConfigurationExpression)
        {
            
        }

        [Test]
        public void Configuration_IsValid()
        {
            _mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}