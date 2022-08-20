using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Examples.ProductService
{
    public static class Utils
    {
        public static DbContextOptions<AppDbContext> GetSqlServerDbContextOptions(DbContextOptionsBuilder<AppDbContext> builder = null)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")!)
                .Options;
        }
    }
}