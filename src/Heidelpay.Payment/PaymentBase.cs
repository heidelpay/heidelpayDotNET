// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="PaymentBase.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Communication.Internal;
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class PaymentBase.
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IRestResource" />
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IHeidelpayProvider" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.Interfaces.IRestResource" />
    /// <seealso cref="Heidelpay.Payment.Interfaces.IHeidelpayProvider" />
    public abstract class PaymentBase : IRestResource, IHeidelpayProvider
    {
        /// <summary>
        /// Gets the type URL.
        /// </summary>
        /// <value>The type URL.</value>
        [JsonIgnore]
        public abstract string TypeUrl { get; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }
                      
        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty]
        public Message Message { get; internal set; }


        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        [JsonProperty]
        internal DateTime? Date { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentBase"/> class.
        /// </summary>
        internal PaymentBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentBase"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        internal PaymentBase(IHeidelpay heidelpayClient)
        {
            Check.ThrowIfNull(heidelpayClient, nameof(heidelpayClient));

            ((IHeidelpayProvider)this).Heidelpay = heidelpayClient;
        }

        /// <summary>
        /// Gets or sets the heidelpay.
        /// </summary>
        /// <value>The heidelpay.</value>
        IHeidelpay IHeidelpayProvider.Heidelpay { get; set; }

        /// <summary>
        /// Gets the heidelpay.
        /// </summary>
        /// <value>The heidelpay.</value>
        protected IHeidelpay Heidelpay { get => ((IHeidelpayProvider)this).Heidelpay; }
    }
}
