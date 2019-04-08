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

            return InternalResolveResourceUrl(value.GetType());
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

            return InternalResolvePaymentUrl(value.GetType(), paymentId);
        }

        /// <summary>
        /// Resolves the resource URL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.String.</returns>
        public static string ResolveResourceUrl<T>()
        {
            return InternalResolveResourceUrl(typeof(T));
        }

        /// <summary>
        /// Resolves the payment URL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>System.String.</returns>
        public static string ResolvePaymentUrl<T>(string paymentId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            return InternalResolvePaymentUrl(typeof(T), paymentId);
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

            return InternalResolveRefundUrl(paymentId, chargeId);
        }

        /// <summary>
        /// Internals the resolve resource URL.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <returns>System.String.</returns>
        private static string InternalResolveResourceUrl(Type resourceType)
        {
            return HeidelpayRegistry.GetPath(resourceType)
                .Replace(PLACEHOLDER_PAYMENT_ID + "/", string.Empty)
                .EnsureTrailingSlash();
        }

        /// <summary>
        /// Internals the resolve payment URL.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>System.String.</returns>
        public static string InternalResolvePaymentUrl(Type resourceType, string paymentId)
        {
            return HeidelpayRegistry.GetPath(resourceType)
                .Replace(PLACEHOLDER_PAYMENT_ID, paymentId)
                .EnsureTrailingSlash();
        }
        /// <summary>
        /// Internals the resolve refund URL.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <returns>System.String.</returns>
        public static string InternalResolveRefundUrl(string paymentId, string chargeId)
        {
            return REFUND_URL
                .Replace(PLACEHOLDER_PAYMENT_ID, paymentId)
                .Replace(PLACEHOLDER_CHARGE_ID, chargeId)
                .EnsureTrailingSlash();
        }
    }
}
