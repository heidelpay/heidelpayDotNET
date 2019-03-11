using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    public class Payment : PaymentBase
    {
        public Payment(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public override string TypeUrl => "payments";
    }

    public enum State
    {
        Completed,
        Pending,
        Canceled,
        Partly,
        Payment_review,
        Chargeback,
    }
}
