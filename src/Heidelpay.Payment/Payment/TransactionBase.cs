﻿// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="PaymentTransactionBase.cs" company="Heidelpay">
//     Copyright (c) 2019 Heidelpay GmbH. All rights reserved.
// </copyright>
// ***********************************************************************
// Licensed under the Apache License, Version 2.0 (the “License”);
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an “AS IS” BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ***********************************************************************

using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class PaymentTransactionBase.
    /// Implements the <see cref="Heidelpay.Payment.PaymentBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentBase" />
    public abstract class TransactionBase : PaymentBase
    {
        /*
         * Java Equivalent is AbstractInitPayment
         */

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public decimal? Amount { get; set; }
        
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }
        
        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        /// <value>The return URL.</value>

        public Uri ReturnUrl { get; set; }
        
        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>The redirect URL.</value>
        public Uri RedirectUrl { get; set; }

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
        /// Gets or sets a value indicating whether this <see cref="Charge"/> is card3ds.
        /// </summary>
        /// <value><c>null</c> if [card3ds] contains no value, <c>true</c> if [card3ds]; otherwise, <c>false</c>.</value>
        public bool? Card3ds { get; set; }

        /// <summary>
        /// Gets or sets the payment reference.
        /// </summary>
        /// <value>
        /// The payment reference.
        /// </value>
        public string PaymentReference { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        public string OrderId { get; set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonIgnore]
        public Status Status
        {
            get
            {
                if (IsSuccess.HasValue && IsSuccess.Value)
                    return Status.Success;

                if (IsPending.HasValue && IsPending.Value)
                    return Status.Pending;

                if (IsError.HasValue && IsError.Value)
                    return Status.Error;

                return Status.Undefined;
            }
        }

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
            internal set
            {
                Resources.TypeId = value;
            }
        }

        /// <summary>
        /// Gets or sets the basket identifier.
        /// </summary>
        /// <value>The basket identifier.</value>
        [JsonIgnore]
        public string BasketId
        {
            get
            {
                return Payment?.BasketId ?? Resources?.BasketId;
            }
            set
            {
                if (Payment == null)
                {
                    Resources.BasketId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        [JsonIgnore]
        public string CustomerId
        {
            get
            {
                return Payment?.CustomerId ?? Resources?.CustomerId;
            }
            set
            {
                if (Payment == null)
                {
                    Resources.CustomerId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the metadata identifier.
        /// </summary>
        /// <value>The metadata identifier.</value>
        [JsonIgnore]
        public string MetadataId
        {
            get
            {
                return Payment?.MetadataId ?? Resources?.MetadataId;
            }
            set
            {
                if (Payment == null)
                {
                    Resources.MetadataId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the risk identifier.
        /// </summary>
        /// <value>The risk identifier.</value>
        [JsonIgnore]
        public string RiskId
        {
            get
            {
                return Payment?.RiskId ?? Resources?.RiskId;
            }
            set
            {
                if (Payment == null)
                {
                    Resources.RiskId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the cancel list.
        /// </summary>
        /// <value>The cancel list.</value>
        [JsonIgnore]
        public IEnumerable<Cancel> CancelList { get; internal set; } = Enumerable.Empty<Cancel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionBase"/> class.
        /// </summary>
        public TransactionBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionBase"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client.</param>
        /// <param name="paymentTypeId">The payment type identifier.</param>
        internal TransactionBase(IHeidelpay heidelpayClient, string paymentTypeId = null)
            : base(heidelpayClient)
        {
            Resources.TypeId = paymentTypeId;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is success.
        /// </summary>
        /// <value><c>true</c> if this instance is success; otherwise, <c>false</c>.</value>
        [JsonProperty]
        internal bool? IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pending.
        /// </summary>
        /// <value><c>true</c> if this instance is pending; otherwise, <c>false</c>.</value>
        [JsonProperty]
        internal bool? IsPending { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is error.
        /// </summary>
        /// <value><c>true</c> if this instance is error; otherwise, <c>false</c>.</value>
        [JsonProperty]
        internal bool? IsError { get; set; }

        /// <summary>
        /// Gets or sets the type of the transaction.
        /// </summary>
        /// <value>The type of the transaction.</value>
        [JsonProperty(PropertyName = "type")]
        internal string TransactionType { get; set; }

        /// <summary>
        /// Gets or sets the processing.
        /// </summary>
        /// <value>The processing.</value>
        [JsonProperty]
        public Processing Processing { get; internal set; }

        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        /// <value>The resources.</value>
        [JsonProperty]
        internal Resources Resources { get; private set; } = new Resources();
    }
}
