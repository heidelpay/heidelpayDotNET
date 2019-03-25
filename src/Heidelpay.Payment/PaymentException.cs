using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Heidelpay.Payment
{
    public sealed class PaymentException : Exception
    {
        public IEnumerable<PaymentError> PaymentErrorList { get; }
        public DateTime Timestamp { get; }
        public Uri Uri { get; }
        public HttpStatusCode StatusCode { get; }

        public PaymentException(Uri uri, HttpStatusCode statusCode, DateTime timestamp, IEnumerable<PaymentError> errors)
            : base(ToMessage(uri, statusCode, errors))
        {
            Timestamp = timestamp;
            Uri = uri;
            PaymentErrorList = errors;
            StatusCode = statusCode;
        }

        public PaymentException(string merchantMessage, string customerMessage, string code, Uri uri)
            : base(merchantMessage)
        {
            PaymentErrorList = new[] { new PaymentError(merchantMessage, customerMessage, code) };
        }

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
