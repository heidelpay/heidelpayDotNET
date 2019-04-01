using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    public class Resources
    {
        internal Resources()
        {

        }

        [JsonProperty]
        public string TypeId { get; internal set; }

        [JsonProperty]
        public string CustomerId { get; internal set; }

        [JsonProperty]
        public string MetadataId { get; internal set; }

        [JsonProperty]
        public string PaymentId { get; internal set; }

        [JsonProperty]
        public string RiskId { get; internal set; }

        [JsonProperty]
        public string BasketId { get; internal set; }
    }
}
