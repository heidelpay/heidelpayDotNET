using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    public class Processing
    {
        [JsonConstructor]
        internal Processing()
        {

        }

        [JsonProperty]
        public string UniqueId { get; internal set; }

        [JsonProperty]
        public string ShortId { get; internal set; }
    }
}
