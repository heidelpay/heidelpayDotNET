// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-15-2019
// ***********************************************************************
// <copyright file="Check.cs" company="Heidelpay">
//     Copyright (c) 2019 Heidelpay GmbH. All rights reserved.
// </copyright>
// ***********************************************************************
// Licensed under the Apache License, Version 2.0 (the “License”);
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an “AS IS” BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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
        /// Throws if null or white space.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="merchantMessage">The merchant message.</param>
        /// <param name="customerMessage">The customer message.</param>
        /// <param name="code">The code.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <exception cref="PaymentException"></exception>
        public static void ThrowIfNullOrWhiteSpace(string obj, string merchantMessage = null, string customerMessage = null, string code = null, Uri returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(obj))
            {
                throw new PaymentException(merchantMessage, customerMessage, code, returnUrl);
            }
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
