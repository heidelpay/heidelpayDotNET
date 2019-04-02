using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Paypal : PaymentTypeBase, IAuthorizedPaymentType, IChargeablePaymentType
    {
        public Paypal()
        {

        }

        internal Paypal(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "types/paypal";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
        Heidelpay IAuthorizedPaymentType.Heidelpay => Heidelpay;
    }
}
