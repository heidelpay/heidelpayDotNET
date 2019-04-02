// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 04-01-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="PaymentTransactionBase.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;


namespace Heidelpay.Payment
{
    /// <summary>
    /// Class PaymentTransactionBase.
    /// Implements the <see cref="Heidelpay.Payment.PaymentBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentBase" />
    public abstract class PaymentTransactionBase : PaymentBase
    {
        /// <summary>
        /// Gets the payment.
        /// </summary>
        /// <value>The payment.</value>
        [JsonProperty]
        public Payment Payment { get; internal set; }

        /// <summary>
        /// Gets the resource URL.
        /// </summary>
        /// <value>The resource URL.</value>
        [JsonProperty]
        public Uri ResourceUrl { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTransactionBase"/> class.
        /// </summary>
        public PaymentTransactionBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTransactionBase"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay.</param>
        internal PaymentTransactionBase(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        /// <summary>
        /// Gets or sets the type of the transaction.
        /// </summary>
        /// <value>The type of the transaction.</value>
        [JsonProperty(PropertyName = "type")]
        internal string TransactionType { get; set; }

        /// <summary>
        /// Gets the payment identifier.
        /// </summary>
        /// <value>The payment identifier.</value>
        [JsonIgnore]
        public string PaymentId
        {
            get
            {
                return Payment?.Id ?? Resources?.PaymentId;
            }
        }

        /// <summary>
        /// Gets the type identifier.
        /// </summary>
        /// <value>The type identifier.</value>
        [JsonIgnore]
        public string TypeId
        {
            get
            {
                return Resources?.TypeId;
            }
        }

        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        /// <value>The resources.</value>
        [JsonProperty]
        internal Resources Resources { get; set; } = new Resources();
    }
}
