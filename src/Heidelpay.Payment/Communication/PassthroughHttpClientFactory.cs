using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Heidelpay.Payment.Communication
{
    internal sealed class PassthroughHttpClientFactory : IHttpClientFactory
    {
        readonly HttpClient client;

        public PassthroughHttpClientFactory(HttpClient client)
        {
            this.client = client;
        }

        public HttpClient CreateClient(string name)
        {
            return this.client;
        }
    }
}
