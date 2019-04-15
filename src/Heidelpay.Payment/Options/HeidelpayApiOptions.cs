// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-15-2019
// ***********************************************************************
// <copyright file="HeidelpayApiOptions.cs" company="Heidelpay">
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
using System.Globalization;
using System.Linq;

namespace Heidelpay.Payment.Options
{
    /// <summary>
    /// Class HeidelpayApiOptions.
    /// </summary>
    public class HeidelpayApiOptions
    {
        /// <summary>
        /// Gets the default API endpoint.
        /// </summary>
        /// <value>The default API endpoint.</value>
        public static Uri DefaultApiEndpoint { get; } = new Uri("https://api.heidelpay.com");
        /// <summary>
        /// Gets the default API version.
        /// </summary>
        /// <value>The default API version.</value>
        public static string DefaultApiVersion { get; } = "v1";

        /// <summary>
        /// Gets the default locale.
        /// </summary>
        /// <value>The default locale.</value>
        public static string DefaultLocale { get; } = "en-US";

        /// <summary>
        /// Gets or sets the API endpoint.
        /// </summary>
        /// <value>The API endpoint.</value>
        public Uri ApiEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the API version.
        /// </summary>
        /// <value>The API version.</value>
        public string ApiVersion { get; set; }

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>The API key.</value>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the HTTP client.
        /// </summary>
        /// <value>The name of the HTTP client.</value>
        public string HttpClientName { get; set; }

        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        /// <value>The locale.</value>
        public string Locale { get; set; }

        internal static HeidelpayApiOptions BuildDefault(string key)
        {
            return new HeidelpayApiOptions
            {
                ApiKey = key,
                ApiEndpoint = DefaultApiEndpoint,
                ApiVersion = DefaultApiVersion,
                Locale = DefaultLocale,
            };
        }

        internal void ThrowIfInvalid()
        {
            Check.ThrowIfNullOrWhiteSpace(ApiKey, 
                "PrivateKey/PublicKey is missing", "An error occured.", "API.000.000.001");

            Check.ThrowIfFalse(string.IsNullOrWhiteSpace(Locale) || DoesCultureExist(Locale), 
                "Options contain invalid configuration values");

            Check.ThrowIfFalse(HttpClientName == null || !string.IsNullOrWhiteSpace(HttpClientName), 
                "Options contain invalid configuration values");
        }

        private static bool DoesCultureExist(string cultureName)
        {
            return CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Any(culture => string.Equals(culture.Name, cultureName, StringComparison.CurrentCultureIgnoreCase));
        }

        internal HeidelpayApiOptions EnsureDefaultSet()
        {
            if (ApiEndpoint == null)
                ApiEndpoint = DefaultApiEndpoint;

            if (string.IsNullOrWhiteSpace(ApiVersion))
                ApiVersion = DefaultApiVersion;

            if (string.IsNullOrWhiteSpace(Locale))
                Locale = DefaultLocale;

            return this;
        }
    }
}
