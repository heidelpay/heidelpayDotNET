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
    public class RestClient : IRestClient
    {
        private readonly JsonSerializerSettings serializationSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        private readonly IHttpClientFactory factory;
        private readonly IOptions<HeidelpayApiOptions> apiOptions;
        private readonly ILogger<RestClient> logger;

        public HeidelpayApiOptions Options { get => apiOptions?.Value; } 

        public RestClient(IHttpClientFactory factory, IOptions<HeidelpayApiOptions> apiOptions, ILogger<RestClient> logger)
        {
            this.factory = factory;
            this.apiOptions = apiOptions;
            this.logger = logger;
        }

        public async Task<object> HttpGetAsync(Uri uri, Type type)
        {
            Check.NotNull(uri, nameof(uri));

            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Get));
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject(content, type);
        }

        public async Task<T> HttpGetAsync<T>(Uri uri)
        {
            Check.NotNull(uri, nameof(uri));

            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Get));
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<T> HttpPostAsync<T>(Uri uri, object data)
        {
            Check.NotNull(uri, nameof(uri));

            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Post, data));
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<T> HttpPutAsync<T>(Uri uri, object data)
        {
            Check.NotNull(uri, nameof(uri));

            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Put, data));
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<bool> HttpDeleteAsync<T>(Uri uri)
        {
            Check.NotNull(uri, nameof(uri));

            var response = await SendRequestAsync(CreateRequest(uri, HttpMethod.Delete));
            var content = await response.Content.ReadAsStringAsync();

            Check.ThrowIfTrue(!string.Equals("true", content, StringComparison.InvariantCultureIgnoreCase),
                $"{typeof(T).Name} '{uri.Segments.Last()}' cannot be deleted");

            return true;
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
            var resolvedFactory = (factory ?? this.factory);

            return !string.IsNullOrWhiteSpace(apiOptions?.Value?.HttpClientName)
                ? resolvedFactory.CreateClient(apiOptions.Value.HttpClientName)
                : resolvedFactory.CreateClient();
        }

        protected async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
        {
            Check.NotNull(request, nameof(request));

            request.AddUserAgent(GetType().FullName);
            request.AddAuthentication(apiOptions?.Value?.ApiKey);

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
                var serializedContent = JsonConvert.SerializeObject(content, serializationSettings);
                request.Content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
                request.Content.Headers.ContentEncoding.Add("UTF-8");
            }

            return request;
        }
    }
}
