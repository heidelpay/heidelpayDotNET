using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Przelewy24 : PaymentTypeBase, IChargeablePaymentType
    {
        [JsonConstructor]
        internal Przelewy24()
        {

        }
                
        public Przelewy24(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/przelewy24";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
