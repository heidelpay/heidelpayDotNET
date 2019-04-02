using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Card : PaymentTypeBase, IChargeablePaymentType, IAuthorizedPaymentType
    {
        public string Number { get; set; }
        public string CVC { get; set; }
        public string ExpiryDate { get; set; }
        public string Brand { get; set; }

        [JsonProperty(PropertyName = "3ds")]
        public bool? ThreeDs { get; set; }

        public Card()
        {

        }

        internal Card(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "types/card";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
        Heidelpay IAuthorizedPaymentType.Heidelpay => Heidelpay;
    }
}
