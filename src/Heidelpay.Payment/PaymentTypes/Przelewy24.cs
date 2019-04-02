using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Przelewy24 : PaymentTypeBase, IChargeablePaymentType
    {
        public Przelewy24()
        {

        }

        internal Przelewy24(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/przelewy24";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
