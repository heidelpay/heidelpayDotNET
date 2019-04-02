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
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }
        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        /// <value>The return URL.</value>
        public Uri ReturnUrl { get; set; }
        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>The redirect URL.</value>
        public Uri RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Authorization"/> is card3ds.
        /// </summary>
        /// <value><c>null</c> if [card3ds] contains no value, <c>true</c> if [card3ds]; otherwise, <c>false</c>.</value>
        public bool? Card3ds { get; set; }

        /// <summary>
        /// Gets or sets the cancel list.
        /// </summary>
        /// <value>The cancel list.</value>
        public IEnumerable<Cancel> CancelList { get; set; } = Enumerable.Empty<Cancel>();

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
            Resources.TypeId = paymentAuthorizable.Id;
        }

        /// <summary>
        /// Gets or sets the processing.
        /// </summary>
        /// <value>The processing.</value>
        [JsonProperty]
        internal Processing Processing { get; set; } = new Processing();

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
