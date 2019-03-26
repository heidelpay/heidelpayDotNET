using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.Communication.Internal
{
    internal class Transaction
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public Uri Url { get; set; }
        public decimal Amount { get; set; }
    }
}
