using Heidelpay.Payment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Prepayment : PaymentTypeBase, IPaymentAuthorize
    {
        public override string TypeUrl => "types/prepayment";

        Heidelpay IPaymentAuthorize.Heidelpay => Heidelpay;
    }
}
