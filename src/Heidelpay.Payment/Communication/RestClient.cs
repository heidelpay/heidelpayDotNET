using Heidelpay.Payment.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Communication
{
    public class RestClient : IRestClient
    {
        private readonly IHttpClientFactory factory;
        private readonly ILogger<RestClient> logger;
        private readonly IOptions<SDKOptions> options;

        private readonly string namedHttpClientInstance;

        public RestClient(string namedHttpClientInstance, 
            IHttpClientFactory factory, IOptions<SDKOptions> options, ILogger<RestClient> logger)
            : this(factory, options, logger)
        {
            this.namedHttpClientInstance = namedHttpClientInstance;
        }

        public RestClient(
            IHttpClientFactory factory, IOptions<SDKOptions> options, ILogger<RestClient> logger)
        {
            this.factory = factory;
            this.logger = logger;
            this.options = options;
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

		    throw new PaymentException(new Uri(error.Url), response.StatusCode, DateTime.Parse(error.Timestamp), error.Errors);
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

        //public async Task<TResponse> HttpGetAsync<TResponse>(Uri uri, string privateKey)
        //{
        //    var responseContent = await HttpGetAsync(uri, privateKey);
        //    return JsonConvert.DeserializeObject<TResponse>(responseContent);
        //}

        public async Task<string> HttpGetAsync(Uri uri, string privateKey)
        {
            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Get), privateKey);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> HttpPostAsync(Uri uri, string privateKey, object data)
        {
            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Post, data), privateKey);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> HttpPutAsync(Uri uri, string privateKey, object data)
        {
            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Put, data), privateKey);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> HttpDeleteAsync(Uri uri, string privateKey)
        {
            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Delete), privateKey);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
