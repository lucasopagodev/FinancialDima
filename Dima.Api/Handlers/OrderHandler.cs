using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;

namespace Dima.Api.Handlers;

public class OrderHandler(AppDbContext context, IStripeHandler stripeHandler) : IOrderHandler
{
    public Task<Response<Order?>> CancelAsync(CancelOrderRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Order?>> PayAsync(PayOrderRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Order?>> RefundAsync(RefundOrderRequest request)
    {
        throw new NotImplementedException();
    }
}
