using Dima.Api.Data;
using Dima.Api.Data.Mappings;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class OrderHandler(AppDbContext context) : IOrderHandler
{
    public async Task<Response<Order?>> CancelAsync(CancelOrderRequest request)
    {
        Order? order;

        try
        {
            order = await context.Orders.Include(x => x.Product).Include(x => x.Voucher).FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (order is null)
                return new Response<Order?>(null, 404, "Pedido não encontrado.");
        }
        catch
        {
            return new Response<Order?>(null, 500, "Falha ao obter pedido");
        }

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(order, 400, "Este pedido já esta cancelado.");

            case EOrderStatus.WaitingPayment:
                break;

            case EOrderStatus.Paid:
                return new Response<Order?>(order, 400, "Este pedido já foi pago e não pode ser cancelado.");

            case EOrderStatus.Refunded:
                return new Response<Order?>(order, 400, "Este pedido já foi reembolsado e não pode ser cancelado.");

            default:
                return new Response<Order?>(order, 400, "Este pedido não pode ser cancelado.");
        }

        order.Status = EOrderStatus.Canceled;
        order.UpdateAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new Response<Order?>(order, 500, "Não foi possível cancelar o seu pedido.");
        }

        return new Response<Order?>(order, 200, $"Pedido {order.Number} cancelado com sucesso.");
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
