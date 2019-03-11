using Heidelpay.Payment.PaymentTypes;
using Heidelpay.Payment.RestClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    public abstract class PaymentBase : IRestResource
    {
        public Heidelpay Heidelpay { get; set; }

        public Payment Payment { get; set; }

        public abstract string TypeUrl { get; }

        public string Id { get; set; }

        public string Type { get; set; }

        public Uri ResourceUri { get; set; }

        public PaymentBase()
        {
        }

        public PaymentBase(Heidelpay heidelpay)
        {
            Heidelpay = heidelpay;
        }

    }
}
