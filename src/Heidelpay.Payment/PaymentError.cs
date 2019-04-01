using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    public class PaymentError
    {
        [JsonProperty]
        public string MerchantMessage { get; internal set; }

        [JsonProperty]
        public string CustomerMessage { get; internal set; }

        [JsonProperty]
        public string Code { get; internal set; }

        [JsonConstructor]
        internal PaymentError()
        {

        }

        public PaymentError(string merchantMessage, string customerMessage, string code)
        {
            MerchantMessage = merchantMessage;
            CustomerMessage = customerMessage;
            Code = code;
        }

        public override string ToString()
        {
            return $"{{merchantMessage: {MerchantMessage}, customerMessage: {CustomerMessage}, code: {Code}}}";
        }
    }
}
