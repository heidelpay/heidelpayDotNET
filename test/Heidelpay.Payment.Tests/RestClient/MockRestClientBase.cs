using Heidelpay.Payment.Communication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace Heidelpay.Payment.Tests.Communication
{
    class MockRestClient : RestClient
    {
        public MockRestClient(string mockedHttpClientName, IHttpClientFactory factory, IOptions<SDKOptions> options, ILogger<RestClient> logger)
            : base(mockedHttpClientName, factory, options, logger)
        {

        }

        public MockRestClient(IHttpClientFactory factory, IOptions<SDKOptions> options, ILogger<RestClient> logger)
            : base(factory, options, logger)
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
