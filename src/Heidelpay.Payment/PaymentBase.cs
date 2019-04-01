﻿using Heidelpay.Payment.Communication.Internal;
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;

namespace Heidelpay.Payment
{
    public abstract class PaymentBase : IRestResource, IHeidelpayProvider
    {
        public Payment Payment { get; internal set; }

        [JsonIgnore]
        public abstract string TypeUrl { get; }

        public string Id { get; set; }

        [JsonProperty]
        internal Uri ResourceUri { get; set; }

        Heidelpay IHeidelpayProvider.Heidelpay { get; set; }

        [JsonProperty]
        internal Message Message { get; set; }

        [JsonProperty(PropertyName = "type")]
        internal string TransactionType { get; set; }

        [JsonProperty]
        internal DateTime? Date { get; set; }
        public PaymentBase()
        {
        }

        internal PaymentBase(Heidelpay heidelpay)
        {
            ((IHeidelpayProvider)this).Heidelpay = heidelpay;
        }

        protected Heidelpay Heidelpay { get => ((IHeidelpayProvider)this).Heidelpay; }
    }
}
