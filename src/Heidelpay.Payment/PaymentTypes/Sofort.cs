// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-15-2019
// ***********************************************************************
// <copyright file="Sofort.cs" company="Heidelpay">
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
    /// Class Sofort. This class cannot be inherited.
    /// Implements the <see cref="PaymentTypeBase" />
    /// Implements the <see cref="IChargeablePaymentType" />
    /// </summary>
    /// <seealso cref="PaymentTypeBase" />
    /// <seealso cref="IChargeablePaymentType" />
    public sealed class Sofort : PaymentTypeBase, IChargeablePaymentType
    {
        /// <summary>
        /// Gets or sets the iban.
        /// </summary>
        /// <value>The iban.</value>
        public string Iban { get; set; }

        /// <summary>
        /// Gets or sets the bic.
        /// </summary>
        /// <value>The bic.</value>
        public string Bic { get; set; }

        /// <summary>
        /// Gets or sets the holder.
        /// </summary>
        /// <value>The holder.</value>
        public string Holder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sofort"/> class.
        /// </summary>
        [JsonConstructor]
        internal Sofort()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sofort"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public Sofort(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        /// <summary>
        /// Gets the heidelpay.
        /// </summary>
        /// <value>The heidelpay.</value>
        IHeidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
