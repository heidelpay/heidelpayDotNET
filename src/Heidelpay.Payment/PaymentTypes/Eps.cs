using Heidelpay.Payment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Eps : PaymentTypeBase, IPaymentCharge
    {
        public string Bic { get; set; }

        public override string TypeUrl => "types/eps";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
    }
}
