using Heidelpay.Payment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Giropay : PaymentTypeBase, IPaymentCharge
    {
        public override string TypeUrl => "types/giropay";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
    }
}