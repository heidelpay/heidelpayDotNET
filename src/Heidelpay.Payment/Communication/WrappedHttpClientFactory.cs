// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 03-25-2019
// ***********************************************************************
// <copyright file="WrappedHttpClientFactory.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Net.Http;

namespace Heidelpay.Payment.Communication
{
    /// <summary>
    /// Class WrappedHttpClientFactory. This class cannot be inherited.
    /// Implements the <see cref="System.Net.Http.IHttpClientFactory" />
    /// </summary>
    /// <seealso cref="System.Net.Http.IHttpClientFactory" />
    internal sealed class WrappedHttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// The wrapped client
        /// </summary>
        readonly HttpClient wrappedClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedHttpClientFactory"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public WrappedHttpClientFactory(HttpClient client)
        {
            this.wrappedClient = client;
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>HttpClient.</returns>
        public HttpClient CreateClient(string name)
        {
            return this.wrappedClient;
        }
    }
}
