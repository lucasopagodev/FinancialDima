using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dima.Core.Requests.Orders
{
    public class GetOrderByNumberRequest : Request
    {
        public string Number { get; set; } = string.Empty;
    }
}