using Heidelpay.Payment;
using System;

namespace System
{ 
    internal static class Check
    {
        public static void NotNull(object obj, string parameterName, string message = null)
        {
            if (obj == null)
                throw string.IsNullOrEmpty(message) 
                    ? new ArgumentNullException(parameterName)
                    : new ArgumentNullException(parameterName, message);
        }

        public static void NotNullOrEmpty(string obj, string parameterName, string message = null)
        {
            if (string.IsNullOrWhiteSpace(obj))
                throw string.IsNullOrEmpty(message)
                    ? new ArgumentNullException(parameterName)
                    : new ArgumentNullException(parameterName, message);
        }

        public static void ThrowIfTrue(bool func, string message)
        {
            if(func)
            {
                throw new PaymentException(message);
            }
        }

        public static void ThrowIfFalse(bool func, string message)
        {
            if (!func)
            {
                throw new PaymentException(message);
            }
        }

        public static void ThrowIfTrue(bool func, string merchantMessage = null, string customerMessage = null, string code = null, Uri returnUrl = null)
        {
            if (func)
            {
                throw new PaymentException(merchantMessage, customerMessage, code, returnUrl);
            }
        }
    }
}
