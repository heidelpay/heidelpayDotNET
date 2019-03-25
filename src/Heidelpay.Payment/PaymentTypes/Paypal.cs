using Heidelpay.Payment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Paypal : PaymentTypeBase, IPaymentAuthorize, IPaymentCharge
    {
        public override string TypeUrl => "types/paypal";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
        Heidelpay IPaymentAuthorize.Heidelpay => Heidelpay;
    }
}
