// ***********************************************************************
// Assembly         : Heidelpay.Payment
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
        /// Gets or sets the card details.
        /// </summary>
        /// <value>
        /// The card details.
        /// </value>
        public CardDetails CardDetails { get; set; }

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

    /// <summary>
    /// CardDetails
    /// </summary>
    public class CardDetails
    {
        /// <summary>
        /// Gets or sets the type of the card.
        /// </summary>
        /// <value>
        /// The type of the card.
        /// </value>
        public string CardType { get; set; }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the country iso a2.
        /// </summary>
        /// <value>
        /// The country iso a2.
        /// </value>
        public string CountryIsoA2 { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        /// <value>
        /// The name of the country.
        /// </value>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the name of the issuer.
        /// </summary>
        /// <value>
        /// The name of the issuer.
        /// </value>
        public string IssuerName { get; set; }

        /// <summary>
        /// Gets or sets the issuer URL.
        /// </summary>
        /// <value>
        /// The issuer URL.
        /// </value>
        public string IssuerUrl { get; set; }

        /// <summary>
        /// Gets or sets the issuer phone number.
        /// </summary>
        /// <value>
        /// The issuer phone number.
        /// </value>
        public string IssuerPhoneNumber { get; set; }
    }
}
