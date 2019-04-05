// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="Sofort.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment.PaymentTypes
{
    /// <summary>
    /// Class Sofort. This class cannot be inherited.
    /// Implements the <see cref="PaymentTypeBase" />
    /// Implements the <see cref="IChargeablePaymentType" />
    /// </summary>
    /// <seealso cref="PaymentTypeBase" />
    /// <seealso cref="IChargeablePaymentType" />
    public sealed class Sofort : PaymentTypeBase, IChargeablePaymentType
    {
        /// <summary>
        /// Gets or sets the iban.
        /// </summary>
        /// <value>The iban.</value>
        public string Iban { get; set; }
        /// <summary>
        /// Gets or sets the bic.
        /// </summary>
        /// <value>The bic.</value>
        public string Bic { get; set; }
        /// <summary>
        /// Gets or sets the holder.
        /// </summary>
        /// <value>The holder.</value>
        public string Holder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sofort"/> class.
        /// </summary>
        [JsonConstructor]
        internal Sofort()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sofort"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public Sofort(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {

        }

        /// <summary>
        /// Gets the type URL.
        /// </summary>
        /// <value>The type URL.</value>
        public override string TypeUrl => "types/sofort";

        /// <summary>
        /// Gets the heidelpay.
        /// </summary>
        /// <value>The heidelpay.</value>
        IHeidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
