using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Paypal : PaymentTypeBase, IPaymentAuthorize, IPaymentCharge
    {
        public Paypal()
        {

        }

        public Paypal(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "types/paypal";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
        Heidelpay IPaymentAuthorize.Heidelpay => Heidelpay;
    }
}
