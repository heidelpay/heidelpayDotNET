using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Heidelpay.Payment.Communication
{
    internal sealed class WrappedHttpClientFactory : IHttpClientFactory
    {
        readonly HttpClient wrappedClient;

        public WrappedHttpClientFactory(HttpClient client)
        {
            this.wrappedClient = client;
        }

        public HttpClient CreateClient(string name)
        {
            return this.wrappedClient;
        }
    }
}
