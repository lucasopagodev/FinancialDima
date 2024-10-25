using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dima.Core.Requests.Orders
{
    public class GetVoucherByNumberRequest : Request
    {
        public string Number { get; set; } = string.Empty;   
    }
}