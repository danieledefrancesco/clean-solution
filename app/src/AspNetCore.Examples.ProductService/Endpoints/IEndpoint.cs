using System;

namespace AspNetCore.Examples.ProductService.Endpoints
{
    public interface IEndpoint
    {
        string Patten { get; }
        string[] Methods { get; }
        Delegate Delegate { get; }
    }
}