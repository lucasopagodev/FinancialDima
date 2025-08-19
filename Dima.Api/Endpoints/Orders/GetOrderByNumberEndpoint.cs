using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Orders
{
    public class GetOrderByNumberEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => 
            app.MapGet("/{number}", HandleAsync)
                .WithName("Orders: Get by number")
                .WithSummary("Recupera um pedido pelo número")
                .WithDescription("Recupera um pedido pelo número")
                .WithOrder(6)
                .Produces<Response<Order?>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IOrderHandler handler, string number)
        {
            var request = new GetOrderByNumberRequest 
            {
                Number = number,
                UserId = user.Identity!.Name ?? String.Empty
            };

            var result = await handler.GetByNumberAsync(request);

            return result.IsSuccess 
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}