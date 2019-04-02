// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="PaymentTypeBase.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Interfaces;

namespace Heidelpay.Payment.PaymentTypes
{
    /// <summary>
    /// Class PaymentTypeBase.
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IPaymentType" />
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IHeidelpayProvider" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.Interfaces.IPaymentType" />
    /// <seealso cref="Heidelpay.Payment.Interfaces.IHeidelpayProvider" />
    public abstract class PaymentTypeBase : IPaymentType, IHeidelpayProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeBase"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay.</param>
        public PaymentTypeBase(IHeidelpay heidelpayClient)
        {
            ((IHeidelpayProvider)this).Heidelpay = heidelpayClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeBase"/> class.
        /// </summary>
        public PaymentTypeBase()
        {
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }
        /// <summary>
        /// Gets the type URL.
        /// </summary>
        /// <value>The type URL.</value>
        public abstract string TypeUrl { get; }
        /// <summary>
        /// Gets or sets the heidelpay.
        /// </summary>
        /// <value>The heidelpay.</value>
        IHeidelpay IHeidelpayProvider.Heidelpay { get; set; }

        /// <summary>
        /// Gets the heidelpay.
        /// </summary>
        /// <value>The heidelpay.</value>
        protected IHeidelpay Heidelpay { get => ((IHeidelpayProvider)this).Heidelpay; }
    }
}
