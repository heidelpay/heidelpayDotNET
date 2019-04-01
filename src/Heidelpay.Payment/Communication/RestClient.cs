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

        public async Task<object> HttpGetAsync(Uri uri, Type responseType)
        {
            return JsonConvert.DeserializeObject(await HttpExecute(HttpMethod.Get, uri), responseType); 
        }

        public async Task<T> HttpGetAsync<T>(Uri uri)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(await HttpExecute(HttpMethod.Get, uri));
        }

        public async Task<T> HttpPostAsync<T>(Uri uri, object data)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(await HttpExecute(HttpMethod.Post, uri, data));
        }

        public async Task<object> HttpPostAsync(Uri uri, object data, Type responseType)
        {
            return JsonConvert.DeserializeObject(await HttpExecute(HttpMethod.Post, uri, data), responseType);
        }

        public async Task<T> HttpPutAsync<T>(Uri uri, object data)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(await HttpExecute(HttpMethod.Put, uri, data));
        }

        public async Task<bool> HttpDeleteAsync<T>(Uri uri)
            where T : class
        {
            var content = await HttpExecute(HttpMethod.Delete, uri);

            Check.ThrowIfTrue(!bool.TryParse(content, out bool bl) || !bl, $"{typeof(T).Name} '{uri.Segments.Last()}' cannot be deleted");

            return true;
        }

        private async Task<string> HttpExecute(HttpMethod method, Uri uri, object data = null)
        {
            Check.NotNull(uri, nameof(uri));

            var response = await SendRequestAsync(CreateRequest(uri, method, data));
            var content = await response.Content.ReadAsStringAsync();

            return content; 
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
