using Heidelpay.Payment;
using Heidelpay.Payment.Communication;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpResponseExtensions
    {
        public static readonly HttpStatusCode[] SuccessStatusCodes = new[]
        {
            HttpStatusCode.Created, // 201
            HttpStatusCode.OK       // 200
        };

        public static async Task ThrowIfErroneousResponseAsync(this HttpResponseMessage response)
        {
            Check.NotNull(response, nameof(response));

            if (SuccessStatusCodes.Contains(response.StatusCode))
                return;

            var responseContent = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<RestClientErrorObject>(responseContent);

            throw AsException(error, response.StatusCode);
        }

        internal static PaymentException AsException(RestClientErrorObject error, HttpStatusCode statusCode)
        {
            var uri = Uri.TryCreate(error.Url, UriKind.RelativeOrAbsolute, out Uri outUri) ? outUri : null;
            var dt = CoreExtensions.TryParseDateTime(error.Timestamp, out DateTime outDt) ? outDt : DateTime.Now;
            var errors = error.Errors ?? Enumerable.Empty<PaymentError>();

            return new PaymentException(uri, statusCode, dt, errors);
        }
    }
}
