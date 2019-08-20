// ***********************************************************************
// Assembly         : Heidelpay.Payment

// ***********************************************************************
// <copyright file="HttpResponseExtensions.cs" company="Heidelpay">
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
            Check.ThrowIfNull(response, nameof(response));

            if (SuccessStatusCodes.Contains(response.StatusCode))
                return;

            var responseContent = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<RestClientErrorObject>(responseContent);

            throw AsException(error, response.StatusCode);
        }

        /// <summary>
        /// Returns an error object as respective payment exception.
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
