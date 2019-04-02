using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Pis : PaymentTypeBase, IChargeablePaymentType
    {
        [JsonConstructor]
        internal Pis()
        {

        }

        public Pis(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/pis";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
