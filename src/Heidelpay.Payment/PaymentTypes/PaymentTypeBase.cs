using Heidelpay.Payment.Interfaces;

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
    }
}
