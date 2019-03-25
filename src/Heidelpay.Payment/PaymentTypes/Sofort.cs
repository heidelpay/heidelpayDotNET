using Heidelpay.Payment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Sofort : PaymentTypeBase, IPaymentCharge
    {
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string Holder { get; set; }


        public override string TypeUrl => "types/sofort";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
    }
}
