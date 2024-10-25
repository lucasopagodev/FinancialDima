using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Orders
{
    public class CancelOrderEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => 
            app.MapPost("/{id}/cancel", HandleAsync)
                .WithName("Orders: Cancel order")
                .WithSummary("Cancela um pedido")
                .WithDescription("Cancela um pedido")
                .WithOrder(2)
                .Produces<Response<Order?>>();

        private static async Task<IResult> HandleAsync(IOrderHandler handler, long id, ClaimsPrincipal user)
        {
            var request = new CancelOrderRequest
            {
                Id = id,
                UserId = user.Identity!.Name ?? String.Empty,
            };

            var result = await handler.CancelAsync(request);
            
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
        }
    }

}