using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Tests.Communication
{
    class MockHttpMessageHandler : HttpMessageHandler
    {
        public MockHttpMessageHandler()
        {

        }

        public MockHttpMessageHandler(HttpStatusCode code, string response)
        {
            this.code = code;
            this.response = response;
        }

        private HttpStatusCode code = HttpStatusCode.OK;
        private string response;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            { 
                StatusCode = code,
                Content = new StringContent(response ?? ""),
            });
        }
    }
}
