using Heidelpay.Payment.Interfaces;
using System;

namespace Heidelpay.Payment.PaymentTypes
{
    public abstract class PaymentTypeBase : IPaymentType, IHeidelpayProvider
    {
        internal PaymentTypeBase(Heidelpay heidelpay)
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

        public Charge NewCharge(decimal? amount = null, string currency = null, Uri returnUrl = null, string customerId = null, string orderId = null, string metadataId = null, string basketId = null, bool? card3ds = null)
        {
            return new Charge
            {
                Amount = amount.GetValueOrDefault(),
                Currency = currency,
                OrderId = orderId,
                Card3ds = card3ds,
                ReturnUrl = returnUrl,
                Resources = new Resources
                {
                    TypeId = Id,
                    MetadataId = metadataId,
                    CustomerId = customerId,
                    BasketId = basketId,
                }
            };
        }
    }
}
