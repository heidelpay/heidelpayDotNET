// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="Prepayment.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    /// <summary>
    /// Class Prepayment. This class cannot be inherited.
    /// Implements the <see cref="Heidelpay.Payment.PaymentTypes.PaymentTypeBase" />
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IChargeablePaymentType" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentTypes.PaymentTypeBase" />
    /// <seealso cref="Heidelpay.Payment.Interfaces.IChargeablePaymentType" />
    public sealed class Prepayment : PaymentTypeBase, IChargeablePaymentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Prepayment"/> class.
        /// </summary>
        [JsonConstructor]
        internal Prepayment()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Prepayment"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public Prepayment(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {

        }

        /// <summary>
        /// Gets the heidelpay.
        /// </summary>
        /// <value>The heidelpay.</value>
        IHeidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
