// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="IRestClient.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Options;
using System;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Interfaces
{
    /// <summary>
    /// Interface IRestClient
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>The options.</value>
        HeidelpayApiOptions Options { get; }

        /// <summary>
        /// HTTP GET as an asynchronous operation.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        Task<object> HttpGetAsync(Uri uri, Type responseType);

        /// <summary>
        /// HTTP POST as an asynchronous operation.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="content">The content.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        Task<object> HttpPostAsync(Uri uri, object content, Type responseType);

        /// <summary>
        /// HTTP GET as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> HttpGetAsync<T>(Uri uri) where T : class;

        /// <summary>
        /// HTTP POST as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> HttpPostAsync<T>(Uri uri, object content) where T : class;

        /// <summary>
        /// HTTP PUT as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="content">The content.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> HttpPutAsync<T>(Uri uri, object content) where T : class;

        /// <summary>
        /// HTTP DELETE as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> HttpDeleteAsync<T>(Uri uri) where T : class;
    }
}
