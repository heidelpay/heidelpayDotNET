// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="Check.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment;

namespace System
{
    /// <summary>
    /// Class Check.
    /// </summary>
    internal static class Check
    {
        /// <summary>
        /// Throws if null.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static void ThrowIfNull(object obj, string parameterName, string message = null)
        {
            if (obj == null)
                throw string.IsNullOrEmpty(message) 
                    ? new ArgumentNullException(parameterName)
                    : new ArgumentNullException(parameterName, message);
        }

        /// <summary>
        /// Throws if null or empty.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static void ThrowIfNullOrEmpty(string obj, string parameterName, string message = null)
        {
            if (string.IsNullOrWhiteSpace(obj))
                throw string.IsNullOrEmpty(message)
                    ? new ArgumentNullException(parameterName)
                    : new ArgumentNullException(parameterName, message);
        }

        /// <summary>
        /// Throws if true.
        /// </summary>
        /// <param name="func">if set to <c>true</c> [function].</param>
        /// <param name="message">The message.</param>
        /// <exception cref="PaymentException"></exception>
        public static void ThrowIfTrue(bool func, string message)
        {
            if(func)
            {
                throw new PaymentException(message);
            }
        }

        /// <summary>
        /// Throws if false.
        /// </summary>
        /// <param name="func">if set to <c>false</c> [function].</param>
        /// <param name="message">The message.</param>
        /// <exception cref="PaymentException"></exception>
        public static void ThrowIfFalse(bool? func, string message)
        {
            if (!func.GetValueOrDefault())
            {
                throw new PaymentException(message);
            }
        }

        /// <summary>
        /// Throws if true.
        /// </summary>
        /// <param name="func">if set to <c>true</c> [function].</param>
        /// <param name="merchantMessage">The merchant message.</param>
        /// <param name="customerMessage">The customer message.</param>
        /// <param name="code">The code.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <exception cref="PaymentException"></exception>
        public static void ThrowIfTrue(bool func, string merchantMessage = null, string customerMessage = null, string code = null, Uri returnUrl = null)
        {
            if (func)
            {
                throw new PaymentException(merchantMessage, customerMessage, code, returnUrl);
            }
        }
    }
}
