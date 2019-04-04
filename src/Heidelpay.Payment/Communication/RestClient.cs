// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="RestClient.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Communication
{
    /// <summary>
    /// Class RestClient.
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IRestClient" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.Interfaces.IRestClient" />
    public class RestClient : IRestClient
    {
        /// <summary>
        /// The serialization settings
        /// </summary>
        readonly JsonSerializerSettings SerializationSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        };

        /// <summary>
        /// The factory
        /// </summary>
        private readonly IHttpClientFactory factory;
        /// <summary>
        /// The API options
        /// </summary>
        private readonly IOptions<HeidelpayApiOptions> apiOptions;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<RestClient> logger;

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>The options.</value>
        public HeidelpayApiOptions Options { get => apiOptions?.Value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="apiOptions">The API options.</param>
        /// <param name="logger">The logger.</param>
        public RestClient(IHttpClientFactory factory, IOptions<HeidelpayApiOptions> apiOptions, ILogger<RestClient> logger)
        {
            this.factory = factory;
            this.apiOptions = apiOptions;
            this.logger = logger;
        }

        /// <summary>
        /// HTTP get as an asynchronous operation.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> HttpGetAsync(Uri uri, Type responseType)
        {
            return JsonConvert.DeserializeObject(await HttpExecute(HttpMethod.Get, uri), responseType); 
        }

        /// <summary>
        /// HTTP get as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public async Task<T> HttpGetAsync<T>(Uri uri)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(await HttpExecute(HttpMethod.Get, uri));
        }

        /// <summary>
        /// HTTP post as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="data">The data.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public async Task<T> HttpPostAsync<T>(Uri uri, object data)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(await HttpExecute(HttpMethod.Post, uri, data));
        }

        /// <summary>
        /// HTTP post as an asynchronous operation.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="data">The data.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> HttpPostAsync(Uri uri, object data, Type responseType)
        {
            return JsonConvert.DeserializeObject(await HttpExecute(HttpMethod.Post, uri, data), responseType);
        }

        /// <summary>
        /// HTTP put as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="data">The data.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public async Task<T> HttpPutAsync<T>(Uri uri, object data)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(await HttpExecute(HttpMethod.Put, uri, data));
        }

        /// <summary>
        /// HTTP delete as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> HttpDeleteAsync<T>(Uri uri)
            where T : class
        {
            var content = await HttpExecute(HttpMethod.Delete, uri);

            Check.ThrowIfTrue(!bool.TryParse(content, out bool bl) || !bl, $"{typeof(T).Name} '{uri.Segments.Last()}' cannot be deleted");

            return true;
        }

        /// <summary>
        /// HTTPs the execute.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="data">The data.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> HttpExecute(HttpMethod method, Uri uri, object data = null)
        {
            Check.NotNull(uri, nameof(uri));

            var response = await SendRequestAsync(CreateRequest(uri, method, data));
            var content = await response.Content.ReadAsStringAsync();

            return content; 
        }

        /// <summary>
        /// Logs the request.
        /// </summary>
        /// <param name="request">The request.</param>
        protected virtual void LogRequest(HttpRequestMessage request)
        {
            logger?.LogDebug(request.ToString());
        }

        /// <summary>
        /// Logs the response.
        /// </summary>
        /// <param name="response">The response.</param>
        protected virtual void LogResponse(HttpResponseMessage response)
        {
            logger?.LogDebug(response.ToString());
        }

        /// <summary>
        /// Resolves the HTTP client.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>HttpClient.</returns>
        protected virtual HttpClient ResolveHttpClient(IHttpClientFactory factory)
        {
            var resolvedFactory = (factory ?? this.factory);

            return !string.IsNullOrWhiteSpace(apiOptions?.Value?.HttpClientName)
                ? resolvedFactory.CreateClient(apiOptions.Value.HttpClientName)
                : resolvedFactory.CreateClient();
        }

        /// <summary>
        /// send request as an asynchronous operation.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        protected async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
        {
            Check.NotNull(request, nameof(request));

            request.AddUserAgent(GetType().FullName);
            request.AddAuthentication(apiOptions?.Value?.ApiKey);
            request.AddLocale(apiOptions?.Value?.Locale);

            LogRequest(request);

            var client = ResolveHttpClient(factory);
            var response = await client
                .SendAsync(request);

            LogResponse(response);

            await response.ThrowIfErroneousResponseAsync();

            return response;
        }

        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns>HttpRequestMessage.</returns>
        private HttpRequestMessage CreateRequest(Uri uri, HttpMethod method, object content = null)
        {
            var request = new HttpRequestMessage(method, uri);

            if (content != null)
            {
                var serializedContent = JsonConvert.SerializeObject(content, SerializationSettings);
                request.Content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
                request.Content.Headers.ContentEncoding.Add("UTF-8");
            }

            return request;
        }
    }
}
