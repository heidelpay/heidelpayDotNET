using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Pis : PaymentTypeBase, IChargeablePaymentType
    {
        public Pis()
        {

        }

        internal Pis(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/pis";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
