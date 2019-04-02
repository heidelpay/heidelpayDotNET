// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="HttpRequestExtensions.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
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
            Check.NotNull(request, nameof(request));
            Check.ThrowIfTrue(string.IsNullOrEmpty(privateKey),
                 merchantMessage: "PrivateKey/PublicKey is missing",
                 customerMessage: "There was a problem authenticating your request. Please contact us for more information.",
                 code: "API.000.000.001",
                 returnUrl: request.RequestUri);

            if (!privateKey.EndsWith(":"))
            {
                privateKey = privateKey + ":";
            }

            var privateKeyBase64 = privateKey.EncodeToBase64();
            request.Headers.Add(RestClientConstants.AUTHORIZATION, $"{RestClientConstants.BASIC}{privateKeyBase64}");
        }

        /// <summary>
        /// Adds the user agent.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="callerName">Name of the caller.</param>
        public static void AddUserAgent(this HttpRequestMessage request, string callerName)
        {
            Check.NotNull(request, nameof(request));
            Check.NotNullOrEmpty(callerName, nameof(callerName));

            request.Headers.Add(RestClientConstants.USER_AGENT, $"{RestClientConstants.USER_AGENT_PREFIX}{SDKInfo.Version} - {callerName}");
        }

        /// <summary>
        /// Adds the locale.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="locale">The locale.</param>
        public static void AddLocale(this HttpRequestMessage request, string locale = null)
        {
            Check.NotNull(request, nameof(request));

            request.Headers.Add(RestClientConstants.ACCEPT_LANGUAGE, locale ?? RestClientConstants.ACCEPT_LANGUAGE_DEFAULT_VALUE);
        }
    }
}
