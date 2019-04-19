// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-15-2019
// ***********************************************************************
// <copyright file="Card.cs" company="Heidelpay">
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

namespace Heidelpay.Payment.PaymentTypes
{
    /// <summary>
    /// Class Card. This class cannot be inherited.
    /// Implements the <see cref="Heidelpay.Payment.PaymentTypes.PaymentTypeBase" />
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IChargeablePaymentType" />
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IAuthorizedPaymentType" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentTypes.PaymentTypeBase" />
    /// <seealso cref="Heidelpay.Payment.Interfaces.IChargeablePaymentType" />
    /// <seealso cref="Heidelpay.Payment.Interfaces.IAuthorizedPaymentType" />
    public sealed class Card : PaymentTypeBase, IChargeablePaymentType, IAuthorizedPaymentType, IProvide3DS
    {
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CVC.
        /// </summary>
        /// <value>The CVC.</value>
        public string CVC { get; set; }

        /// <summary>
        /// Gets or sets the expiry date.
        /// </summary>
        /// <value>The expiry date.</value>
        public string ExpiryDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the brand.
        /// </summary>
        /// <value>The brand.</value>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [three ds].
        /// </summary>
        /// <value><c>null</c> if [three ds] contains no value, <c>true</c> if [three ds]; otherwise, <c>false</c>.</value>
        [JsonProperty(PropertyName = "3ds")]
        public bool? ThreeDs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        [JsonConstructor]
        internal Card()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public Card(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }
              
        /// <summary>
        /// Gets the heidelpay client.
        /// </summary>
        /// <value>The heidelpay client.</value>
        IHeidelpay IChargeablePaymentType.Heidelpay => Heidelpay;

        /// <summary>
        /// Gets the heidelpay client.
        /// </summary>
        /// <value>The heidelpay client.</value>
        IHeidelpay IAuthorizedPaymentType.Heidelpay => Heidelpay;
    }
}
