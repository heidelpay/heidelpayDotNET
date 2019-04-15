// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-15-2019
// ***********************************************************************
// <copyright file="HttpRequestExtensions.cs" company="Heidelpay">
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

namespace System.Net.Http
{
    /// <summary>
    /// Class HttpRequestMessageExtensions.
    /// </summary>
    internal static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Adds the authentication.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="privateKey">The private key.</param>
        public static void AddAuthentication(this HttpRequestMessage request, string privateKey)
        {
            Check.ThrowIfNull(request, nameof(request));
            Check.ThrowIfTrue(string.IsNullOrEmpty(privateKey),
                 merchantMessage: "PrivateKey/PublicKey is missing",
                 customerMessage: "There was a problem authenticating your request. Please contact us for more information.",
                 code: "API.000.000.001",
                 returnUrl: request.RequestUri);

            if (!privateKey.EndsWith(":"))
            {
                privateKey += ":";
            }

            var privateKeyBase64 = privateKey.EncodeToBase64();
            request.Headers.Add(RestClientConstants.AUTHORIZATION, $"{RestClientConstants.BASIC} {privateKeyBase64}");
        }

        /// <summary>
        /// Adds the user agent.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="callerName">Name of the caller.</param>
        public static void AddUserAgent(this HttpRequestMessage request, string callerName)
        {
            Check.ThrowIfNull(request, nameof(request));
            Check.ThrowIfNullOrEmpty(callerName, nameof(callerName));

            request.Headers.Add(RestClientConstants.USER_AGENT, $"{RestClientConstants.USER_AGENT_PREFIX}{SDKInfo.Version} - {callerName}");
        }

        /// <summary>
        /// Adds the locale.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="locale">The locale.</param>
        public static void AddLocale(this HttpRequestMessage request, string locale = null)
        {
            Check.ThrowIfNull(request, nameof(request));

            request.Headers.Add(RestClientConstants.ACCEPT_LANGUAGE, locale);
        }
    }
}
