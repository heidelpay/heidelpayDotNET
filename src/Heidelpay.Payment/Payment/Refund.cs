// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Refund.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class Refund.
    /// Implements the <see cref="Heidelpay.Payment.PaymentBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentBase" />
    public class Refund : PaymentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Refund"/> class.
        /// </summary>
        [JsonConstructor]
        internal Refund()
        {

        }

        /// <summary>Initializes a new instance of the <see cref="T:Heidelpay.Payment.Refund"/> class.</summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public Refund(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }
    }
}
