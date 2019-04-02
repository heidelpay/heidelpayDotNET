using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Eps : PaymentTypeBase, IChargeablePaymentType
    {
        public string Bic { get; set; }

        public Eps()
        {

        }

        internal Eps(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/eps";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
