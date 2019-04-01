using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class SepaDirectDebit : PaymentTypeBase, IPaymentCharge
    {
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string Holder { get; set; }

        public SepaDirectDebit()
        {

        }

        internal SepaDirectDebit(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "types/sepa-direct-debit";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
    }
}
