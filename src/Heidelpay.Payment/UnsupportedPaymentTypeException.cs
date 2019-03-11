using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    public sealed class UnsupportedPaymentTypeException : Exception
    {
        public UnsupportedPaymentTypeException(string message)
            : base(message)
        {

        }
    }
}
