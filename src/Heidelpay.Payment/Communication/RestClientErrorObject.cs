using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.Communication
{
    public class RestClientErrorObject
    {
        public string Url { get; set; }
        public string Timestamp { get; set; }
        public List<PaymentError> Errors { get; set; }
    }
}
