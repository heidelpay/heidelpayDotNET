using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Card : PaymentTypeBase, IPaymentCharge, IPaymentAuthorize
    {
        public string Number { get; set; }
        public string CVC { get; set; }
        public string ExpiryDate { get; set; }
        public string Brand { get; set; }

        public Card()
        {

        }

        public Card(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "types/card";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
        Heidelpay IPaymentAuthorize.Heidelpay => Heidelpay;
    }
}
