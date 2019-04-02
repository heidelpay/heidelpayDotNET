using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class SepaDirectDebitGuaranteed : PaymentTypeBase, IChargeablePaymentType
    {
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string Holder { get; set; }

        [JsonConstructor]
        internal SepaDirectDebitGuaranteed()
        {

        }

        public SepaDirectDebitGuaranteed(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "types/sepa-direct-debit-guaranteed";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
