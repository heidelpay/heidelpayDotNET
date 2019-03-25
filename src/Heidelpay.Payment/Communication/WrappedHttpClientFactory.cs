using System.Net.Http;

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
