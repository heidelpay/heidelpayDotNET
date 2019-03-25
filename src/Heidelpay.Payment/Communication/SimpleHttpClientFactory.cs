using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Heidelpay.Payment.Communication
{
    internal sealed class SimpleHttpClientFactory : IHttpClientFactory
    {
        readonly HttpClient client;
        public SimpleHttpClientFactory()
        {
            this.client = new HttpClient();
        }

        public HttpClient CreateClient(string name)
        {
            return this.client;
        }
    }
}
