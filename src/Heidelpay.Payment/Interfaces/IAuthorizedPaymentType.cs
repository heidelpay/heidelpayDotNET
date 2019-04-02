// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="IAuthorizedPaymentType.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Heidelpay.Payment.Interfaces
{
    /// <summary>
    /// Interface IAuthorizedPaymentType
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IPaymentType" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.Interfaces.IPaymentType" />
    public interface IAuthorizedPaymentType : IPaymentType
    {
        /// <summary>
        /// Gets the heidelpay.
        /// </summary>
        /// <value>The heidelpay.</value>
        IHeidelpay Heidelpay { get; }
    }
}
