using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Eps : PaymentTypeBase, IChargeablePaymentType
    {
        public string Bic { get; set; }

        [JsonConstructor]
        internal Eps()
        {

        }

        public Eps(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/eps";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
