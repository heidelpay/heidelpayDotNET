using Heidelpay.Payment.RestClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.PaymentTypes
{
    public abstract class PaymentType : IRestResource
    {
        public PaymentType(Heidelpay heidelpay)
        {
            Heidelpay = heidelpay;
        }

        public PaymentType()
        {
        }

        public Heidelpay Heidelpay { get; set; }
        public string Id { get; set; }
        public abstract string TypeUrl { get; }
    }
}
