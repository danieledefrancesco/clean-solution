using System;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneOf.Types;

namespace AspNetCore.Examples.ProductService.Endpoints
{
    public class GetProductEndpoint : EndpointBase<GetProductRequestDto, GetProductByIdRequest, GetProductByIdResponse,
        ProductDto>
    {
        public override string Patten => "/products/{id}";
        public override string[] Methods => new[] { "GET" };
        public override Delegate Delegate =>
            (
                    [FromBody] request,
                    [FromServices] mapper,
                    [FromServices] mediator,
                    [FromServices] factory,
                    [FromServices] validator) =>
                Handle(request, mapper, mediator, factory, validator);
        internal override IResult CreateResult(ProductDto response) => Results.Ok(response);
    }
}