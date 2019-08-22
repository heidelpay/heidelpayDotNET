// ***********************************************************************
// Assembly         : Heidelpay.Payment

// ***********************************************************************
// <copyright file="PaymentException.cs" company="Heidelpay">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class PaymentException. This class cannot be inherited.
    /// Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class PaymentException : Exception
    {
        /// <summary>
        /// Gets the payment error list.
        /// </summary>
        /// <value>The payment error list.</value>
        public IEnumerable<PaymentError> PaymentErrorList { get; }
        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public DateTime Timestamp { get; }
        /// <summary>
        /// Gets the URI.
        /// </summary>
        /// <value>The URI.</value>
        public Uri Uri { get; }
        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PaymentException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public PaymentException(string message, HttpStatusCode statusCode, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentException"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="errors">The errors.</param>
        public PaymentException(Uri uri, HttpStatusCode statusCode, DateTime timestamp, IEnumerable<PaymentError> errors)
            : base(ToMessage(uri, statusCode, errors))
        {
            Timestamp = timestamp;
            Uri = uri;
            PaymentErrorList = errors;
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentException"/> class.
        /// </summary>
        /// <param name="merchantMessage">The merchant message.</param>
        /// <param name="customerMessage">The customer message.</param>
        /// <param name="code">The code.</param>
        /// <param name="uri">The URI.</param>
        public PaymentException(string merchantMessage, string customerMessage, string code, Uri uri)
            : base(merchantMessage)
        {
            PaymentErrorList = new[] { new PaymentError(merchantMessage, customerMessage, code) };
            Uri = uri;
        }

        /// <summary>
        /// Converts to message.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>System.String.</returns>
        private static string ToMessage(Uri uri, HttpStatusCode statusCode, IEnumerable<PaymentError> errors)
        {
            var sb = new StringBuilder();
            if (uri != null)
            {
                sb.Append("Heidelpay responded with ");
                sb.Append((int)statusCode);
                sb.Append(" when calling ");
                sb.Append(uri);
                sb.Append(".");
            }

            return WithErrors(sb, errors).ToString();
        }

        /// <summary>
        /// Withes the errors.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>StringBuilder.</returns>
        private static StringBuilder WithErrors(StringBuilder sb, IEnumerable<PaymentError> errors)
        {
            if (errors == null || errors.Count() <= 0)
            {
                return sb;
            }
            sb.Append($" [{string.Join(",", errors.Select(x => x.ToString()))}]");

            return sb;
        }
    }
}
