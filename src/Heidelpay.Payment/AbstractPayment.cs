using Heidelpay.Payment.PaymentTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    public abstract class AbstractPayment : IPaymentType
    {
        public AbstractPayment(Heidelpay heidelpay)
        {
            Heidelpay = heidelpay;
        }

        public AbstractPayment()
        {
        }

        public Heidelpay Heidelpay { get; set; }

        public abstract string TypeUrl { get; }

        public string Id { get; set; }

        public string Type { get; set; }

        public Uri ResourceUri { get; set; }
    }
}
