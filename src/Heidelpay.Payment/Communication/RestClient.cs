﻿using Heidelpay.Payment.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Options;

namespace Heidelpay.Payment.Communication
{
    public class RestClient : IRestClient
    {
        private readonly IHttpClientFactory factory;
        private readonly ILogger<RestClient> logger;

        public RestClient(IHttpClientFactory factory, ILogger<RestClient> logger)
        {
            this.factory = factory;
            this.logger = logger;
        }

        protected virtual void LogRequest(HttpRequestMessage request)
        {
            logger?.LogDebug(request.ToString());
        }

        protected virtual void LogResponse(HttpResponseMessage response)
        {
            logger?.LogDebug(response.ToString());
        }

        protected virtual HttpClient ResolveHttpClient(IHttpClientFactory factory)
        {
            return (factory ?? this.factory).CreateClient();
        }

        protected async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, string privateKey)
        {
            Check.NotNull(request, nameof(request));

            request.AddUserAgent(GetType().FullName);
            request.AddAuthentication(privateKey);

            LogRequest(request);

            var client = ResolveHttpClient(factory);
            var response = await client
                .SendAsync(request);

            LogResponse(response);

            await response.ThrowIfErroneousResponseAsync();

            return response;
        }

        private HttpRequestMessage CreateRequest(Uri uri, HttpMethod method, object content = null)
        {
            var request = new HttpRequestMessage(method, uri);

            if (content != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
                request.Content.Headers.ContentEncoding.Add("UTF-8");
            }

            return request;
        }

        public async Task<string> HttpGetAsync(Uri uri, string privateKey)
        {
            Check.NotNull(uri, nameof(uri));

            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Get), privateKey);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> HttpPostAsync(Uri uri, string privateKey, object data)
        {
            Check.NotNull(uri, nameof(uri));

            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Post, data), privateKey);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> HttpPutAsync(Uri uri, string privateKey, object data)
        {
            Check.NotNull(uri, nameof(uri));

            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Put, data), privateKey);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> HttpDeleteAsync(Uri uri, string privateKey)
        {
            Check.NotNull(uri, nameof(uri));

            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Delete), privateKey);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
