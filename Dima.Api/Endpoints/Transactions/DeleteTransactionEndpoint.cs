﻿using System.Transactions;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
    => app.MapDelete("/{id}", HandleAsync)
      .WithName("Transactions: Delete")
      .WithSummary("Exclui uma transação")
      .WithDescription("Exclui uma transação")
      .WithOrder(3)
      .Produces<Response<Transaction?>>();

  private static async Task<IResult> HandleAsync(
    ITransactionHandler handler,
    long id)
  {
    var request = new DeleteTransactionRequest
    {
      UserId = "teste@lucas",
      Id = id
    };

    var result = await handler.DeleteAsync(request);
    return result.IsSuccess
        ? TypedResults.Ok(result)
        : TypedResults.BadRequest(result);
  }
}
