using Newtonsoft.Json;

namespace Heidelpay.Payment.Communication.Internal
{
    public class Message
    {
        internal Message()
        {

        }

        [JsonProperty]
        public string Code { get; internal set; }

        [JsonProperty]
        public string Customer { get; internal set; }

        [JsonProperty]
        public string Merchant { get; internal set; }
    }
}
