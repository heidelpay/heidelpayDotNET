using System;

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
