using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    public class PaymentError
    {
        public PaymentError(string merchantMessage, string customerMessage, string code)
        {
            MerchantMessage = merchantMessage;
            CustomerMessage = customerMessage;
            Code = code;
        }

        public PaymentError()
        {

        }

        public string MerchantMessage { get; set; }
        public string CustomerMessage { get; set; }
        public string Code { get; set; }

        public override string ToString()
        {
            return $"{{merchantMessage: {MerchantMessage}, customerMessage: {CustomerMessage}, code: {Code}}}";
        }
    }
}
