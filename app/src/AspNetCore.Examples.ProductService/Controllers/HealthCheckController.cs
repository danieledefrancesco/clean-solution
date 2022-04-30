using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : AppControllerBase
    {
        public HealthCheckController(IErrorHandlerFactory errorHandlerFactory, IMediator mediator) : base(errorHandlerFactory, mediator)
        {
        }

        [HttpGet]
        public Task<IActionResult> HealthCheck()
        {
            IActionResult result = Ok(
                new HealthCheckDto
                {
                    Success = true
                });
            return Task.FromResult(result);
        }
    }
}