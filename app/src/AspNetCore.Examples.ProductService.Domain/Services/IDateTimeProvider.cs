using System;

namespace AspNetCore.Examples.ProductService.Services
{
    public interface IDateTimeProvider
    {
        DateTime TodayDate { get; }
    }
}