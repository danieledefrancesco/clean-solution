using System;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.Responses;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.Endpoints
{
    public class CreateProductEndpoint : EndpointBase<CreateProductRequestDto, CreateProductCommandRequest,
        CreateProductCommandResponse, ProductDto>
    {
        public override string Patten => "/products";
        public override string[] Methods => new[] { "POST", "PUT" };

        public override Delegate Delegate =>
            (
                    [FromBody] request,
                    [FromServices] mapper,
                    [FromServices] mediator,
                    [FromServices] factory,
                    [FromServices] validator) =>
                Handle(request, mapper, mediator, factory, validator);

        internal override IResult CreateResult(ProductDto response) =>
            Results.Created($"/products/{response.Id}", response);
    }
}