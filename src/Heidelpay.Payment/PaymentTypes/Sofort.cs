using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Sofort : PaymentTypeBase, IChargeablePaymentType
    {
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string Holder { get; set; }

        [JsonConstructor]
        internal Sofort()
        {

        }

        public Sofort(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "types/sofort";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
