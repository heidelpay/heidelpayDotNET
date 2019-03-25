using Heidelpay.Payment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class InvoiceGuaranteed : PaymentTypeBase, IPaymentAuthorize
    {
        public InvoiceGuaranteed()
        {

        }

        public InvoiceGuaranteed(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/invoice-guaranteed";

        Heidelpay IPaymentAuthorize.Heidelpay => Heidelpay;
    }
}
