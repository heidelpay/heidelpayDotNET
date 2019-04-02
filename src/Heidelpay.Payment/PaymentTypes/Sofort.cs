using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Sofort : PaymentTypeBase, IChargeablePaymentType
    {
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string Holder { get; set; }

        public Sofort()
        {

        }

        internal Sofort(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "types/sofort";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
