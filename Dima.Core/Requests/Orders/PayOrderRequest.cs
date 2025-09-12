using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dima.Core.Requests.Orders
{
    public class PayOrderRequest : Request
    {
        public long Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string ExternalReference { get; set; } = string.Empty;
    }
}