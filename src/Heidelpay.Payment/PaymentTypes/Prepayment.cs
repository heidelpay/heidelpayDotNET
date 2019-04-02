using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Prepayment : PaymentTypeBase, IChargeablePaymentType
    {
        [JsonConstructor]
        internal Prepayment()
        {

        }

        public Prepayment(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/prepayment";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
