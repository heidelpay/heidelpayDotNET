using Heidelpay.Payment.Communication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Heidelpay.Payment.Tests.Communication
{
    class MockRestClient : RestClientBase
    {
        public MockRestClient(IHttpClientFactory factory, IOptions<SDKOptions> options, ILogger<RestClientBase> logger)
            : base(factory, options, logger)
        {

        }

        protected override HttpRequestMessage CreateRequest(Uri uri, HttpMethod method)
        {
            return new HttpRequestMessage(method, uri);
        }
    }
}
