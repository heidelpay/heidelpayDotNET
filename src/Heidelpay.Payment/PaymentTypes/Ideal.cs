using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Ideal : PaymentTypeBase, IChargeablePaymentType
    {
        public string Bic { get; set; }

        public Ideal()
        {

        }

        internal Ideal(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "types/ideal";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
