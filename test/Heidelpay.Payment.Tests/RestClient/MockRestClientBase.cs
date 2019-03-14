using Heidelpay.Payment.RestClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Heidelpay.Payment.Tests.Communication
{
    class MockRestClientBase : RestClientBase
    {
        public MockRestClientBase(string mockedHttpClientName, IHttpClientFactory factory, IOptions<SDKOptions> options, ILogger<RestClientBase> logger)
            : base(mockedHttpClientName, factory, options, logger)
        {

        }

        public MockRestClientBase(IHttpClientFactory factory, IOptions<SDKOptions> options, ILogger<RestClientBase> logger)
            : base(factory, options, logger)
        {

        }

        protected override HttpRequestMessage CreateRequest(Uri uri, HttpMethod method)
        {
            return new HttpRequestMessage(method, uri);
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
