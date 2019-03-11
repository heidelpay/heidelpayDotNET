using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Communication
{
    public abstract class RestClientBase : IRestClient
    {
        private IHttpClientFactory factory;
        private ILogger<RestClientBase> logger;
        private IOptions<SDKOptions> options;

        public RestClientBase(IHttpClientFactory factory, IOptions<SDKOptions> options, ILogger<RestClientBase> logger)
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

        protected async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, string privateKey)
        {
            request.AddUserAgent(GetType().FullName);
            request.AddAuthentication(privateKey);

            logger?.LogDebug(request.ToString());

            var response = await factory.CreateClient().SendAsync(request);

            logger?.LogDebug(response.StatusCode.ToString());
            logger?.LogDebug(response.ToString());

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
