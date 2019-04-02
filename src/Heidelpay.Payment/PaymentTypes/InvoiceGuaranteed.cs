using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class InvoiceGuaranteed : PaymentTypeBase, IChargeablePaymentType
    {
        [JsonConstructor]
        internal InvoiceGuaranteed()
        {

        }

        public InvoiceGuaranteed(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/invoice-guaranteed";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
