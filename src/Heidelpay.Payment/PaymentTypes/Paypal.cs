using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Paypal : PaymentTypeBase, IAuthorizedPaymentType, IChargeablePaymentType
    {
        [JsonConstructor]
        internal Paypal()
        {

        }

        public Paypal(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "types/paypal";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
        Heidelpay IAuthorizedPaymentType.Heidelpay => Heidelpay;
    }
}
