using Heidelpay.Payment;
using Heidelpay.Payment.Communication;

namespace System.Net.Http
{
    public static class HttpRequestMessageExtensions
    {
        public static void AddAuthentication(this HttpRequestMessage request, string privateKey)
        {
            Check.NotNull(request, nameof(request));
            Check.ThrowIfTrue(string.IsNullOrEmpty(privateKey),
                 merchantMessage: "PrivateKey/PublicKey is missing",
                 customerMessage: "There was a problem authenticating your request. Please contact us for more information.",
                 code: "API.000.000.001",
                 returnUrl: request.RequestUri);

            if (!privateKey.EndsWith(":"))
            {
                privateKey = privateKey + ":";
            }

            var privateKeyBase64 = privateKey.EncodeToBase64();
            request.Headers.Add(RestClientConstants.AUTHORIZATION, $"{RestClientConstants.BASIC}{privateKeyBase64}");
        }

        public static void AddUserAgent(this HttpRequestMessage request, string callerName)
        {
            Check.NotNull(request, nameof(request));

            request.Headers.Add(RestClientConstants.USER_AGENT, $"{RestClientConstants.USER_AGENT_PREFIX}{SDKInfo.Version} - {callerName}");
        }
    }
}
