using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Invoice : PaymentTypeBase, IPaymentAuthorize
    {
        public Invoice()
        {

        }

        internal Invoice(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/invoice";

        Heidelpay IPaymentAuthorize.Heidelpay => Heidelpay;
    }
}
