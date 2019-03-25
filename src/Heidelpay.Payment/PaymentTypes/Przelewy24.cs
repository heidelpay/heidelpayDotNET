using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Przelewy24 : PaymentTypeBase, IPaymentCharge
    {
        public Przelewy24()
        {

        }

        public Przelewy24(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/przelewy24";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
    }
}
