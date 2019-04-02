// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="HeidelpayApiOptions.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Heidelpay.Payment.Options
{
    /// <summary>
    /// Class HeidelpayApiOptions.
    /// </summary>
    public class HeidelpayApiOptions
    {
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
    }
}
