﻿using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    public sealed class Pis : PaymentTypeBase, IPaymentCharge
    {
        public Pis()
        {

        }

        internal Pis(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }
        public override string TypeUrl => "types/pis";

        Heidelpay IPaymentCharge.Heidelpay => Heidelpay;
    }
}
