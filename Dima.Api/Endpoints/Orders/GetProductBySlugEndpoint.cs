using Azure;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;

namespace Dima.Api.Endpoints.Orders
{
    public class GetProductBySlugEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => 
            app.MapGet("/{slug}", HandleAsync)
            .WithName("Products: Get by slug")
            .WithSummary("Recupera um produto por slug")
            .WithDescription("Recupera um produto por slug")
            .WithOrder(2)
            .Produces<Response<Product?>>();


        private static async Task<IResult> HandleAsync(
            IProductHandler handler,
            string slug)
        {
            var request = new GetProductBySlugRequest
            {
                Slug = slug,
            };

            var result = await handler.GetBySlugAsync(request);

            return result.IsSuccess 
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}