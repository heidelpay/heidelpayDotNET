using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.RestClient
{
    public class RestClientError
    {
        public string Code { get; set; }
        public string MerchantMessage { get; set; }
        public string CustomerMessage { get; set; }
        public string Customer { get; set; }
    }
}
