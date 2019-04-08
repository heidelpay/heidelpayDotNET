// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-28-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="IPaymentTypeExtensions.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
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
        /// <param name="payment">The payment.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public static async Task<Authorization> AuthorizeAsync(this IAuthorizedPaymentType payment, 
            decimal amount, string currency, Uri returnUrl, Customer customer = null)
        {
            Check.ThrowIfNull(payment.Heidelpay, "Heidelpay", "You cannot call authorize methods on an unattached resource. Please either inject or use heidelpay instance directly.");

            return await payment.Heidelpay.AuthorizeAsync(amount, currency, payment, returnUrl, customer);
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public static async Task<Charge> ChargeAsync(this IChargeablePaymentType payment, 
            decimal amount, string currency, Uri returnUrl, Customer customer = null)
        {
            Check.ThrowIfNull(payment.Heidelpay, "Heidelpay", "You cannot call charge methods on an unattached resource. Please either inject or use heidelpay instance directly.");

            return await payment.Heidelpay.ChargeAsync(amount, currency, payment, returnUrl, customer);
        }
    }
}
