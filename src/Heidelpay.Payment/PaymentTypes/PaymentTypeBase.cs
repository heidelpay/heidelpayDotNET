using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;

namespace Heidelpay.Payment.PaymentTypes
{
    public abstract class PaymentTypeBase : IPaymentType, IHeidelpayProvider
    {
        public PaymentTypeBase(Heidelpay heidelpay)
        {
            ((IHeidelpayProvider)this).Heidelpay = heidelpay;
        }

        public PaymentTypeBase()
        {
        }

        public string Id { get; set; }
        public abstract string TypeUrl { get; }
        Heidelpay IHeidelpayProvider.Heidelpay { get; set; }

        protected Heidelpay Heidelpay { get => ((IHeidelpayProvider)this).Heidelpay; }
    }
}
