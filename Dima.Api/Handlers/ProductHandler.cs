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
    public class ProductHandler(AppDbContext context) : IProductHandler
    {
        public async Task<PagedResponse<List<Product>?>> GetAllAsync(GetAllProductRequest request)
        {
            try
            {
                var query = context.Products.AsNoTracking().Where(p => p.IsActive == true).OrderBy(P => P.Title);

                var products = await query.Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

                var count = await query.CountAsync();

                return new PagedResponse<List<Product>?>(products, count, request.PageNumber, request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Product>?>(null, 500, "Não foi possível consultar os produtos");
            }
        }

        public async Task<Response<Product?>> GetBySlugAsync(GetProductBySlugRequest request)
        {
            try
            {
                var product = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Slug == request.Slug && p.IsActive == true);

                return product is null ? new Response<Product?>(null, 404, "Produto não encotrado") : new Response<Product?>(product);
            }
            catch
            {
               return new Response<Product?>(null, 500, "Não foi possível recuperar o produto");
            }
        }
    }
}