using Dima.Core.Models;
using Dima.Core.Responses;
using Dima.Core.Requests.Orders;

namespace Dima.Core.Handlers
{
    public interface IProductHandler
    {
        Task<PagedResponse<List<Product>>> GetAllAsync(GetAllProductRequest request);
        Task<Response<Product?>> GetBySlugAsync(GetProductBySlugRequest request);
    }
}