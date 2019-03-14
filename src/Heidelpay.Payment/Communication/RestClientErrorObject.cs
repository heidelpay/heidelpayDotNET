using System.Collections.Generic;

namespace Heidelpay.Payment.Communication
{
    public class RestClientErrorObject
    {
        public string Url { get; set; }
        public string Timestamp { get; set; }
        public List<PaymentError> Errors { get; set; }
    }
}
