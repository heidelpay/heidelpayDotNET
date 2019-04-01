using System;

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
