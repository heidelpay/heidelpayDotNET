// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="IPaymentTypeExtensions.cs" company="Heidelpay">
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
using System;
using System.Threading.Tasks;

namespace Heidelpay.Payment.PaymentTypes
{
    /// <summary>
    /// Class IPaymentTypeExtensions.
    /// </summary>
    public static class IPaymentTypeExtensions
    {
        /// <summary>
        /// authorize as an asynchronous operation.
        /// </summary>
        /// <param name="paymentType">The payment.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public static async Task<Authorization> AuthorizeAsync(this IAuthorizedPaymentType paymentType, 
            decimal amount, string currency, Uri returnUrl, Customer customer = null)
        {
            Check.ThrowIfNull(paymentType.Heidelpay, "Heidelpay", "You cannot call authorize methods on an unattached resource. Please either inject or use heidelpay instance directly.");

            return await paymentType.Heidelpay.AuthorizeAsync(amount, currency, paymentType, returnUrl, customer);
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="paymentType">The payment.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public static async Task<Charge> ChargeAsync(this IChargeablePaymentType paymentType, 
            decimal amount, string currency, Uri returnUrl, Customer customer = null)
        {
            Check.ThrowIfNull(paymentType.Heidelpay, "Heidelpay", "You cannot call charge methods on an unattached resource. Please either inject or use heidelpay instance directly.");

            return await paymentType.Heidelpay.ChargeAsync(amount, currency, paymentType, returnUrl, customer);
        }
    }
}
