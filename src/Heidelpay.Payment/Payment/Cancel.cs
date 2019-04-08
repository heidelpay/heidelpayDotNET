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
    public sealed class Cancel : PaymentTransactionBase
    {
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
    }
}
