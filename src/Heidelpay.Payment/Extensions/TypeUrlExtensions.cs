// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="TypeUrlExtensions.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Heidelpay.Payment.Interfaces
{
    /// <summary>
    /// Class TypeUrlExtensions.
    /// </summary>
    internal static class TypeUrlExtensions
    {
        /// <summary>
        /// The placeholder charge identifier
        /// </summary>
        private const string PLACEHOLDER_CHARGE_ID = "<chargeId>";

        /// <summary>
        /// The placeholder payment identifier
        /// </summary>
        private const string PLACEHOLDER_PAYMENT_ID = "<paymentId>";

        /// <summary>
        /// The refund URL
        /// </summary>
        private const string REFUND_URL = "payments/<paymentId>/charges/<chargeId>/cancels";

        /// <summary>
        /// Resolves the resource URL.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string ResolveResourceUrl(this IRestResource value)
        {
            Check.ThrowIfNull(value, nameof(value));
            
            return HeidelpayRegistry.GetPath(value.GetType())
                .Replace(PLACEHOLDER_PAYMENT_ID + "/", string.Empty)
                .EnsureTrailingSlash();
        }

        /// <summary>
        /// Resolves the payment URL.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>System.String.</returns>
        public static string ResolvePaymentUrl(this IRestResource value, string paymentId)
        {
            Check.ThrowIfNull(value, nameof(value));
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            return HeidelpayRegistry.GetPath(value.GetType())
                .Replace(PLACEHOLDER_PAYMENT_ID, paymentId)
                .EnsureTrailingSlash();
        }
        /// <summary>
        /// Resolves the refund URL.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <returns>System.String.</returns>
        public static string ResolveRefundUrl(this IRestResource value, string paymentId, string chargeId)
        {
            Check.ThrowIfNull(value, nameof(value));
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            Check.ThrowIfNullOrEmpty(chargeId, nameof(chargeId));

            return REFUND_URL
                .Replace(PLACEHOLDER_PAYMENT_ID, paymentId)
                .Replace(PLACEHOLDER_CHARGE_ID, chargeId)
                .EnsureTrailingSlash();
        }
    }
}
