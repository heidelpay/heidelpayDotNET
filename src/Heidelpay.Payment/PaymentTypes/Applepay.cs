// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-15-2019
// ***********************************************************************
// <copyright file="Applepay.cs" company="Heidelpay">
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
    /// Class Applepay. This class cannot be inherited.
    /// Implements the <see cref="Heidelpay.Payment.PaymentTypes.PaymentTypeBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentTypes.PaymentTypeBase" />
    public sealed class Applepay : PaymentTypeBase, IAuthorizedPaymentType, IChargeablePaymentType
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the signature.
        /// </summary>
        /// <value>The signature.</value>
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public ApplepayHeader Header { get; set; } = new ApplepayHeader();

        public string Number { get; set; }

        public string ExpiryDate { get; set; }

        public string CurrencyCode { get; set; }

        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Applepay" /> class.
        /// </summary>
        [JsonConstructor]
        internal Applepay()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Applepay" /> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public Applepay(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        IHeidelpay IAuthorizedPaymentType.Heidelpay => Heidelpay;
        IHeidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }

    /// <summary>
    /// Class ApplepayHeader. This class cannot be inherited.
    /// </summary>
    public sealed class ApplepayHeader
    {
        /// <summary>
        /// Gets or sets the ephemeral public key.
        /// </summary>
        /// <value>The ephemeral public key.</value>
        public string EphemeralPublicKey { get; set; }

        /// <summary>
        /// Gets or sets the public key hash.
        /// </summary>
        /// <value>The public key hash.</value>
        public string PublicKeyHash { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        public string TransactionId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplepayHeader"/> class.
        /// </summary>
        public ApplepayHeader()
        {
        }
    }
}
