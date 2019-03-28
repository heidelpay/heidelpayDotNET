using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Heidelpay.Payment
{
    public class MetaData : IRestResource, IHeidelpayProvider
    {
        public string Id { get; set; }

        [JsonProperty]
        internal IDictionary<string, string> MetadataMap { get; set; } = new Dictionary<string, string>();

        public MetaData() 
            : this(false)
        {
        }

        public MetaData(bool sorted)
        {
            if(sorted)
            {
                MetadataMap = new SortedDictionary<string, string>();
            }
        }

        public string this[string key]
        {
            get
            {
                return MetadataMap[key];
            }
            set
            {
                MetadataMap[key] = value;
            }
        }

        public int Count
        {
            get
            {
                return MetadataMap.Count;
            }
        }

        public bool ContainsKey(string key)
        {
            return MetadataMap.ContainsKey(key);
        }

        public string TypeUrl => "metadata";

        Heidelpay IHeidelpayProvider.Heidelpay { get; set; }
    }
}
