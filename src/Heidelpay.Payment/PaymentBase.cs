using Heidelpay.Payment.Communication.Internal;
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;

namespace Heidelpay.Payment
{
    public abstract class PaymentBase : IRestResource, IHeidelpayProvider
    {
        [JsonIgnore]
        public abstract string TypeUrl { get; }

        public string Id { get; set; }

        

        Heidelpay IHeidelpayProvider.Heidelpay { get; set; }

        [JsonProperty]
        public Message Message { get; internal set; }
                

        [JsonProperty]
        internal DateTime? Date { get; set; }

        public PaymentBase()
        {
        }

        internal PaymentBase(Heidelpay heidelpay)
        {
            Check.NotNull(heidelpay, nameof(heidelpay));

            ((IHeidelpayProvider)this).Heidelpay = heidelpay;
        }

        protected Heidelpay Heidelpay { get => ((IHeidelpayProvider)this).Heidelpay; }
    }
}
