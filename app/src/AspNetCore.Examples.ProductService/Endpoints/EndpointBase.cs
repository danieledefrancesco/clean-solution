using System;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Requests;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using OneOf;

namespace AspNetCore.Examples.ProductService.Endpoints
{
    public abstract class EndpointBase<TRequest, TAppRequest, TAppResponse, TResponse> : IEndpoint
    where TAppRequest: IAppRequest<TAppResponse>
    {
        public abstract string Patten { get; }
        public abstract string[] Methods { get; }
        public abstract Delegate Delegate { get; }

        internal async Task<IResult> Handle(
            TRequest request,
            IMapper mapper,
            IMediator mediator,
            IErrorHandlerFactory errorHandlerFactory,
            IValidator<TRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }
            var appRequest = mapper.Map<TAppRequest>(request);
            var appResponse = await mediator.Send(appRequest);
            if (appResponse!.Value is IError error)
            {
                return errorHandlerFactory.GetSupportingHandler(error).HandleError(error);
            }
            var response = mapper.Map<TResponse>(appResponse.Value);
            return CreateResult(response);
        }

        internal abstract IResult CreateResult(TResponse response);
    }
}