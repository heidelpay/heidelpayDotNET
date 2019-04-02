using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Prepayment : PaymentTypeBase, IAuthorizedPaymentType
    {
        public Prepayment()
        {

        }

        internal Prepayment(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/prepayment";

        Heidelpay IAuthorizedPaymentType.Heidelpay => Heidelpay;
    }
}
