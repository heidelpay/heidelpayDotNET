using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public abstract class PaymentTypeBase : IPaymentType
    {
        public PaymentTypeBase(Heidelpay heidelpay)
        {
            Heidelpay = heidelpay;
        }

        public PaymentTypeBase()
        {
        }

        internal Heidelpay Heidelpay { get; set; }

        public string Id { get; set; }
        public abstract string TypeUrl { get; }
    }
}
