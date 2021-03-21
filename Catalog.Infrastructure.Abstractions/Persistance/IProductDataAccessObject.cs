using System;
using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Abstractions.DataAccessLayer
{
    public interface IProductDataAccessObject : IDataAccessObject<Product,Guid>
    {
        
    }
}