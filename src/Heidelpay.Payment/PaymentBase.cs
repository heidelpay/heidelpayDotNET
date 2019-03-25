using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Interfaces;
using System;

namespace Heidelpay.Payment
{
    public abstract class PaymentBase : IRestResource, IHeidelpayProvider
    {
        public Payment Payment { get; set; }

        public abstract string TypeUrl { get; }

        public string Id { get; set; }

        public string Type { get; set; }

        public Uri ResourceUri { get; set; }
        Heidelpay IHeidelpayProvider.Heidelpay { get; set; }

        public PaymentBase()
        {
        }

        public PaymentBase(Heidelpay heidelpay)
        {
            ((IHeidelpayProvider)this).Heidelpay = heidelpay;
        }

        protected Heidelpay Heidelpay { get => ((IHeidelpayProvider)this).Heidelpay; }
    }
}
