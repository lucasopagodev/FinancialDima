using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dima.Core.Requests.Orders
{
    public class GetProductBySlugRequest : Request
    {
        public string Slug { get; set; } = string.Empty;
    }
}