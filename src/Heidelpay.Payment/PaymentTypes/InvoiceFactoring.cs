using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class InvoiceFactoring : PaymentTypeBase, IChargeablePaymentType
    {
        [JsonConstructor]
        internal InvoiceFactoring()
        {

        }

        public InvoiceFactoring(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, Uri returnUrl, Customer customer, Basket basket, string invoiceId = null)
        {
            return await Heidelpay.ChargeAsync(amount, currency, this, returnUrl, customer, basket, invoiceId);
        }

        public override string TypeUrl => "types/invoice-factoring";

        Heidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
