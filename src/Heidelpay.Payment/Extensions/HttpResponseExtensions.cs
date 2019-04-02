// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="HttpResponseExtensions.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment;
using Heidelpay.Payment.Communication;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    /// Class HttpResponseExtensions.
    /// </summary>
    internal static class HttpResponseExtensions
    {
        /// <summary>
        /// The success status codes
        /// </summary>
        public static readonly HttpStatusCode[] SuccessStatusCodes = new[]
        {
            HttpStatusCode.Created, // 201
            HttpStatusCode.OK       // 200
        };

        /// <summary>
        /// throw if erroneous response as an asynchronous operation.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>Task.</returns>
        public static async Task ThrowIfErroneousResponseAsync(this HttpResponseMessage response)
        {
            Check.NotNull(response, nameof(response));

            if (SuccessStatusCodes.Contains(response.StatusCode))
                return;

            var responseContent = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<RestClientErrorObject>(responseContent);

            throw AsException(error, response.StatusCode);
        }

        /// <summary>
        /// Ases the exception.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="statusCode">The status code.</param>
        /// <returns>PaymentException.</returns>
        internal static PaymentException AsException(RestClientErrorObject error, HttpStatusCode statusCode)
        {
            var uri = Uri.TryCreate(error.Url, UriKind.RelativeOrAbsolute, out Uri outUri) ? outUri : null;
            var dt = CoreExtensions.TryParseDateTime(error.Timestamp, out DateTime outDt) ? outDt : DateTime.Now;
            var errors = error.Errors ?? Enumerable.Empty<PaymentError>();

            return new PaymentException(uri, statusCode, dt, errors);
        }
    }
}
