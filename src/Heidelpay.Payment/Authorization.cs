// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="Authorization.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Authorization : PaymentTransactionBase
    {
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
            : this (paymentAuthorizable.Heidelpay)
        {
            if (paymentAuthorizable is IProvide3DS threeDSprovider)
            {
                Card3ds = threeDSprovider.ThreeDs;
            }
            Resources.TypeId = paymentAuthorizable.Id;
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

        /// <summary>
        /// Gets the type URL.
        /// </summary>
        /// <value>The type URL.</value>
        public override string TypeUrl => "payments/<paymentId>/authorize";
    }
}
