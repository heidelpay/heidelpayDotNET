// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Shipment.cs" company="Heidelpay">
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

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class Shipment.
    /// Implements the <see cref="Heidelpay.Payment.PaymentBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentBase" />
    public class Shipment : PaymentBase
    {
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public string InvoiceId { get; set; }

        /// <summary>
        /// Gets the payment.
        /// </summary>
        /// <value>The payment.</value>
        [JsonProperty]
        public Payment Payment { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Refund"/> class.
        /// </summary>
        [JsonConstructor]
        internal Shipment()
        {

        }

        /// <summary>Initializes a new instance of the <see cref="T:Heidelpay.Payment.Shipment"/> class.</summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public Shipment(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        /// <value>The resources.</value>
        [JsonProperty]
        internal Resources Resources { get; set; } = new Resources();
    }
}
