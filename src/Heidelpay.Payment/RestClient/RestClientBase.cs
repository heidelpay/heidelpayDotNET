using Heidelpay.Payment.RestClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Heidelpay.Payment.RestClient
{
    public abstract class RestClientBase : IRestClient
    {
        private readonly IHttpClientFactory factory;
        private readonly ILogger<RestClientBase> logger;
        private readonly IOptions<SDKOptions> options;

        private readonly string namedHttpClientInstance;

        public RestClientBase(string namedHttpClientInstance, 
            IHttpClientFactory factory, IOptions<SDKOptions> options, ILogger<RestClientBase> logger)
            : this(factory, options, logger)
        {
            this.namedHttpClientInstance = namedHttpClientInstance;
        }

        public RestClientBase(
            IHttpClientFactory factory, IOptions<SDKOptions> options, ILogger<RestClientBase> logger)
        {
            this.factory = factory;
            this.logger = logger;
            this.options = options;
        }

        public async Task<TResponse> HttpGetAsync<TResponse>(Uri uri, string privateKey)
        {
            var responseContent = await HttpGetAsync(uri, privateKey);
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }

        public async Task<string> HttpGetAsync(Uri uri, string privateKey)
        {
            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Get), privateKey);
            return await response.Content.ReadAsStringAsync();
        }

        protected virtual void LogRequest(HttpRequestMessage request)
        {
            logger?.LogDebug(request.ToString());
        }

        protected virtual void LogResponse(HttpResponseMessage response)
        {
            logger?.LogDebug(response.ToString());
        }

        protected async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, string privateKey)
        {
            request.AddUserAgent(GetType().FullName);
            request.AddAuthentication(privateKey);

            LogRequest(request);

            var response = await factory.CreateClient(namedHttpClientInstance)
                .SendAsync(request);

            LogResponse(response);

            if (response.IsError())
            {
                await ThrowPaymentExceptionAsync(request, response);
            }

            return response;
        }

        protected async Task ThrowPaymentExceptionAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<RestClientErrorObject>(responseContent);

		    throw new PaymentException(new Uri(error.Url), (int)response.StatusCode, DateTime.Parse(error.Timestamp), error.Errors);
        }

        protected abstract HttpRequestMessage CreateRequest(Uri uri, HttpMethod method);
    }
}
