// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-15-2019
// ***********************************************************************
// <copyright file="Authorization.cs" company="Heidelpay">
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
using System.Threading.Tasks;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Business object for Authorization. Amount, currency and typeId are mandatory parameter to 
    /// execute an Authorization.
    /// 
    /// The returnUrl is mandatory in case of redirectPayments like Sofort, Paypal, Giropay, Creditcard 3DS
    /// 
    /// Implements the <see cref="Heidelpay.Payment.PaymentTransactionBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentTransactionBase" />
    public sealed class Authorization : PaymentTransactionBase
    {
        public decimal EffectiveInterestRate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Authorization"/> class.
        /// </summary>
        [JsonConstructor]
        internal Authorization()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Authorization"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        internal Authorization(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Authorization"/> class.
        /// </summary>
        /// <param name="paymentAuthorizable">The payment authorizable.</param>
        public Authorization(IAuthorizedPaymentType paymentAuthorizable)
            : base(paymentAuthorizable.Heidelpay, paymentTypeId: paymentAuthorizable.Id)
        {
            if (paymentAuthorizable is IProvide3DS threeDSprovider)
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
            return await Heidelpay.CancelAuthorizationAsync(Payment?.Id ?? Resources?.PaymentId, amount);
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(decimal? amount = null)
        {
            return await Heidelpay.ChargeAuthorizationAsync(Payment?.Id ?? Resources?.PaymentId, amount);
        }
    }
}
