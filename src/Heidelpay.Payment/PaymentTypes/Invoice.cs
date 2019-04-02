using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Invoice : PaymentTypeBase, IChargeablePaymentType
    {
        [JsonConstructor]
        internal Invoice()
        {

        }
        
        public Invoice(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/invoice";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
