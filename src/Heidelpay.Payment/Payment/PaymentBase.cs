// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="PaymentBase.cs" company="Heidelpay">
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
        /*
         * Java Equivalent is AbstractPayment
         */

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
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
        /// Gets or sets the heidelpay client.
        /// </summary>
        /// <value>The heidelpay client.</value>
        IHeidelpay IHeidelpayProvider.Heidelpay { get; set; }

        /// <summary>
        /// Gets the heidelpay client.
        /// </summary>
        /// <value>The heidelpay client.</value>
        protected IHeidelpay Heidelpay { get => ((IHeidelpayProvider)this).Heidelpay; }
    }
}
