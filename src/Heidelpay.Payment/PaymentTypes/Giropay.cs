using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Giropay : PaymentTypeBase, IPaymentCharge
    {
        public Giropay()
        {

        }

        internal Giropay(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/giropay";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
    }
}