using System;

namespace AspNetCore.Examples.ProductService.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TransactionAttribute: Attribute
    {
        
    }
}