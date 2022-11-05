using System;

namespace AspNetCore.Examples.ProductService.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TransactionAttribute: Attribute
    {
        
    }
}