using Heidelpay.Payment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Przelewy24 : PaymentTypeBase, IPaymentCharge
    {
        public override string TypeUrl => "types/przelewy24";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
    }
}
