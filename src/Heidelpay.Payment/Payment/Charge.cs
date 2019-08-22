// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="Charge.cs" company="Heidelpay">
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
using System.Linq;
using System.Threading.Tasks;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Business object for Charge. Amount, currency and typeId are mandatory parameter to 
    /// execute an Charge.
    /// 
    /// The returnUrl is mandatory in case of redirectPayments like Sofort, Paypal, Giropay, Creditcard 3DS
    /// 
    /// Implements the <see cref="Heidelpay.Payment.TransactionBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.TransactionBase" />
    public sealed class Charge : TransactionBase
    {
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public string InvoiceId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Charge"/> class.
        /// </summary>
        [JsonConstructor]
        internal Charge()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Charge"/> class.
        /// </summary>
        /// <param name="chargeablePayment">The chargeable payment.</param>
        public Charge(IChargeablePaymentType chargeablePayment)
            : base(chargeablePayment.Heidelpay, paymentTypeId: chargeablePayment.Id)
        {
            if (chargeablePayment is IProvide3DS threeDSprovider)
            {
                Card3ds = threeDSprovider.ThreeDs;
            }
        }

        /// <summary>
        /// cancel as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> CancelAsync(decimal? amount = null)
        {
            return await Heidelpay.CancelChargeAsync(Payment.Id, Id, amount);
        }

        /// <summary>
        /// Cancels the asynchronous.
        /// </summary>
        /// <param name="amountGross">The amount gross.</param>
        /// <param name="amountNet">The amount net.</param>
        /// <param name="amountVat">The amount vat.</param>
        /// <returns></returns>
        public async Task<Cancel> CancelAsync(decimal amountGross, decimal amountNet, decimal amountVat)
        {
            return await Heidelpay.CancelChargeAsync(Payment.Id, Id, amountGross, amountNet, amountVat);
        }

        /// <summary>
        /// Gets the cancel.
        /// </summary>
        /// <param name="cancelId">The cancel identifier.</param>
        /// <returns>Cancel.</returns>
        public Cancel GetCancel(string cancelId)
        {
            return CancelList?.FirstOrDefault(x => string.Equals(x.Id, cancelId, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    /// <summary>
    /// Enum Status
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Success
        /// </summary>
        Success,
        /// <summary>
        /// Pending
        /// </summary>
        Pending,
        /// <summary>
        /// Error
        /// </summary>
        Error,
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,
    }
}
