using Heidelpay.Payment.Communication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Heidelpay.Payment.Options;

namespace Heidelpay.Payment.Tests.Communication
{
    class MockRestClient : RestClient
    {
        protected override HttpClient ResolveHttpClient(IHttpClientFactory factory)
        {
            return factory.CreateClient("MockHttpMessageHandler");
        }

        public MockRestClient(IHttpClientFactory factory, ILogger<RestClient> logger)
            : base(factory, logger)
        {
        }

        public HttpRequestMessage LoggedRequest { get; private set; }
        protected override void LogRequest(HttpRequestMessage request)
        {
            LoggedRequest = request;
        }

        public HttpResponseMessage LoggedResponse { get; private set; }
        protected override void LogResponse(HttpResponseMessage response)
        {
            LoggedResponse = response;
        }
    }
}
