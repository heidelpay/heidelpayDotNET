using Heidelpay.Payment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Invoice : PaymentTypeBase, IPaymentAuthorize
    {
        public override string TypeUrl => "types/invoice";

        Heidelpay IPaymentAuthorize.Heidelpay => Heidelpay;
    }
}
