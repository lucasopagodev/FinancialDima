using System.Net.Mime;
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

    public async Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
    {
        Product? product;
        try
        {
            product = await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.ProductId && x.IsActive == true);

            if (product is null)
                return new Response<Order?>(null, 400, "Produto não encontrado.");

            context.Attach(product);
        }
        catch
        {
            return new Response<Order?>(null, 500, "Não foi possível obter o produto.");
        }

        Voucher? voucher = null;
        try
        {
            if (request.VoucherId is not null)
            {
                voucher = await context.Vouchers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.VoucherId && x.IsActive == true);

                if (voucher is null)
                    return new Response<Order?>(null, 400, "Voucher inválido ou não encontrado.");

                if (voucher.IsActive == false)
                    return new Response<Order?>(null, 400, "Este voucher já foi utilizado.");

                voucher.IsActive = false;
                context.Vouchers.Update(voucher);
            }
        }
        catch 
        {
            return new Response<Order?>(null, 500, "Falha ao obter o voucher informado.");
        }

        var order = new Order 
        {
            UserId = request.UserId,
            Product = product,
            ProductId = request.ProductId,
            Voucher = voucher,
            VoucherId = request.VoucherId
        };

        try
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
        catch 
        {
            return new Response<Order?>(null, 500, "Não foi possível realizar o seu pedido.");
        }

        return new Response<Order?>(order, 201, $"Pedido {order.Number} cadastrado com sucesso.");
    }

    public async Task<Response<Order?>> PayAsync(PayOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context.Orders.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (order is null)
                return new Response<Order?>(null, 404, "Pedido não encontrado");
        }
        catch 
        {
            return new Response<Order?>(null, 500, "Falha ao consultar pedido.");
        }

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(order, 400, "Este pedido já foi cancelado e não pode ser pago.");
            
            case EOrderStatus.Paid:
                return new Response<Order?>(order, 400, "Este pedido já está pago.");
            
            case EOrderStatus.Refunded:
                return new Response<Order?>(order, 400, "Este pedido já foi reembolsado e não pode ser pago.");

            case EOrderStatus.WaitingPayment:
                break;

            default:
                return new Response<Order?>(order, 400, "Não foi possível pagar o pedido.");
        }

        order.Status = EOrderStatus.Paid;
        order.ExternalReference = request.ExternalReference;
        order.UpdateAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch 
        {
            return new Response<Order?>(null, 500, "Falha ao tentar pagar o pedido.");
        }

        return new Response<Order?>(order, 200, $"Pedido {order.Number} pago com sucesso.");
    }

    public async Task<Response<Order?>> RefundAsync(RefundOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context.Orders.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (order is null)
                return new Response<Order?>(null, 404, "Pedido não encontrado.");
        }
        catch 
        {
            return new Response<Order?>(null, 500, "Falha ao consultar seu pedido");
        }

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(null, 400, "O pedido está cancelado!");
            
            case EOrderStatus.WaitingPayment:
                return new Response<Order?>(null, 400, "O pedido ainda não foi pago.");

            case EOrderStatus.Refunded:
                return new Response<Order?>(null, 400, "O pedido já foi reembolsado.");

            case EOrderStatus.Paid:
                break;

            default:
                return new Response<Order?>(null, 400, "Situação do pedido inválida.");
        }

        order.Status = EOrderStatus.Refunded;
        order.UpdateAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new Response<Order?>(null, 500, "Não foi possível reembolsar seu pedido.");
        }

        return new Response<Order?>(order, 200, $"Pedido {order.Number} estornado com sucesso!");
    }
    
    public async Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
    {
        try
        {
            var query = context.Orders.AsNoTracking()
                .Include(p => p.Product)
                .Include(v => v.Voucher)
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.CreatedAt);

            var orders = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Order>?>(
                orders,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Order>?>(null, 500, "Não foi possível consultar os pedidos");
        }
    }

    public async Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
    {
        try
        {
            var order = await context
                .Orders
                .AsNoTracking()
                .Include(x => x.Product)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(x => x.Number == request.Number && x.UserId == request.UserId);

            return order is null
                ? new Response<Order?>(null, 404, "Pedido não encontrado")
                : new Response<Order?>(order);
        }
        catch
        {
            return new Response<Order?>(null, 500, "Não foi possível recuperar o pedido");
        }
    }
}
