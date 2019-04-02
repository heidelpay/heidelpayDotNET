// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Cancel.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Business object for Cancellations
    /// 
    /// Implements the <see cref="Heidelpay.Payment.PaymentTransactionBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentTransactionBase" />
    public class Cancel : PaymentTransactionBase
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Gets or sets the processing.
        /// </summary>
        /// <value>The processing.</value>
        [JsonProperty]
        internal Processing Processing { get; set; } = new Processing();

        /// <summary>
        /// Initializes a new instance of the <see cref="Cancel"/> class.
        /// </summary>
        [JsonConstructor]
        internal Cancel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cancel"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public Cancel(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        /// <summary>
        /// Gets the type URL.
        /// </summary>
        /// <value>The type URL.</value>
        public override string TypeUrl => "payments/<paymentId>/authorize/cancels";
    }
}
