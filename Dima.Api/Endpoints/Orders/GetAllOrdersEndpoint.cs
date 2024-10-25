using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Orders
{
    public class GetAllOrdersEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => 
            app.MapGet("/", HandleAsync)
                .WithName("Orders: Get all")
                .WithSummary("Recupera todos os pedidos")
                .WithDescription("Recupera todos os pedidos")
                .WithOrder(5)
                .Produces<List<Order>?>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            IOrderHandler handler,
            [FromQuery] int pagedNumber = Configuration.DefaultPageNumber,
            [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetAllOrdersRequest
            {
                UserId = user.Identity!.Name ?? String.Empty,
                PageNumber = pagedNumber,
                PageSize = pageSize
            };

            var result = await handler.GetAllAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}