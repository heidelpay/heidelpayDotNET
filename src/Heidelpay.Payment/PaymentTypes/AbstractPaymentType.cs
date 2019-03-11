using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.PaymentTypes
{
    public abstract class AbstractPaymentType : IPaymentType
    {
        public AbstractPaymentType(Heidelpay heidelpay)
        {
            Heidelpay = heidelpay;
        }

        public AbstractPaymentType()
        {

        }

        public Heidelpay Heidelpay { get; set; }
        public string Id { get; set; }

        public abstract string TypeUrl { get; }
    }
}
