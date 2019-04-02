using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Ideal : PaymentTypeBase, IChargeablePaymentType
    {
        public string Bic { get; set; }

        [JsonConstructor]
        internal Ideal()
        {

        }
        
        public Ideal(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "types/ideal";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
