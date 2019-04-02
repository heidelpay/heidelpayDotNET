using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Giropay : PaymentTypeBase, IChargeablePaymentType
    {
        [JsonConstructor]
        internal Giropay()
        {

        }
                
        public Giropay(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/giropay";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}