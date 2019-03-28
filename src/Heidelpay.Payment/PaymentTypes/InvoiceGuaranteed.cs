using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class InvoiceGuaranteed : PaymentTypeBase, IPaymentCharge
    {
        public InvoiceGuaranteed()
        {

        }

        internal InvoiceGuaranteed(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/invoice-guaranteed";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
    }
}
