using Heidelpay.Payment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Pis : PaymentTypeBase, IPaymentCharge
    {
        public override string TypeUrl => "types/pis";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
    }
}
