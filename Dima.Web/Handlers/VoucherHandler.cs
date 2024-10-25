using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;

namespace Dima.Web.Handlers
{
    public class VoucherHandler(IHttpClientFactory httpClientFactory) : IVoucherHandler
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
        public async Task<Response<Voucher?>> GetByNumberAsync(GetVoucherByNumberRequest request)
            => await _client.GetFromJsonAsync<Response<Voucher?>>($"v1/voucher/{request.Number}") ?? new Response<Voucher?>(null, 400, "Não foi possível obter o voucher.");
    }
}