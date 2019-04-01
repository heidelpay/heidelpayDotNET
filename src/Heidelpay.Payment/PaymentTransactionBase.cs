using Heidelpay.Payment.Communication.Internal;
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;


namespace Heidelpay.Payment
{
    public abstract class PaymentTransactionBase : PaymentBase
    {
        [JsonProperty]
        public Payment Payment { get; internal set; }

        [JsonProperty]
        public Uri ResourceUrl { get; internal set; }

        public PaymentTransactionBase()
        {
        }

        internal PaymentTransactionBase(Heidelpay heidelpay)
            : base(heidelpay)
        {
        }
        
        [JsonProperty(PropertyName = "type")]
        internal string TransactionType { get; set; }

        [JsonIgnore]
        public string PaymentId
        {
            get
            {
                return Payment?.Id ?? Resources?.PaymentId;
            }
        }

        [JsonIgnore]
        public string TypeId
        {
            get
            {
                return Resources?.TypeId;
            }
        }

        [JsonProperty]
        internal Resources Resources { get; set; } = new Resources();
    }
}
