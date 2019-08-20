// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="InvoiceFactoring.cs" company="Heidelpay">
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
using System.Threading.Tasks;

namespace Heidelpay.Payment.PaymentTypes
{
    /// <summary>
    /// Class InvoiceFactoring. This class cannot be inherited.
    /// Implements the <see cref="Heidelpay.Payment.PaymentTypes.PaymentTypeBase" />
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IChargeablePaymentType" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentTypes.PaymentTypeBase" />
    /// <seealso cref="Heidelpay.Payment.Interfaces.IChargeablePaymentType" />
    public sealed class InvoiceFactoring : PaymentTypeBase, IChargeablePaymentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceFactoring"/> class.
        /// </summary>
        [JsonConstructor]
        internal InvoiceFactoring()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceFactoring"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public InvoiceFactoring(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="basket">The basket.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(decimal amount, string currency, Uri returnUrl, Customer customer, Basket basket, string invoiceId = null)
        {
            return await Heidelpay.ChargeAsync(amount, currency, this, returnUrl, customer, basket, invoiceId);
        }

        /// <summary>
        /// Gets the heidelpay client.
        /// </summary>
        /// <value>The heidelpay client.</value>
        IHeidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
