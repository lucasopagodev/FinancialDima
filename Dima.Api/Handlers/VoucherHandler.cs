using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
    public class VoucherHandler(AppDbContext context) : IVoucherHandler
    {
        public async Task<Response<Voucher?>> GetByNumberAsync(GetVoucherByNumberRequest request)
        {
            try
            {
                var voucher = await context.Vouchers.AsNoTracking().FirstOrDefaultAsync(x => x.Number == request.Number && x.IsActive == true);

                return voucher is null ? new Response<Voucher?>(null, 404, "Voucher não encontrado!") : new Response<Voucher?>(voucher);
            }
            catch (System.Exception)
            {
                return new Response<Voucher?>(null, 500, "Não foi possível encontrar seu voucher.");
            }
        }
    }
}