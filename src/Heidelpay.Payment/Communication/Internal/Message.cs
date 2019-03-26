using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.Communication.Internal
{
    internal class Message
    {
        public string Code { get; set; }
        public string Customer { get; set; }
        public string Merchant { get; set; }
    }
}
