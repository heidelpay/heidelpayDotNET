// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="IHeidelpayProvider.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Heidelpay.Payment.Interfaces
{
    /// <summary>
    /// Interface IHeidelpayProvider
    /// </summary>
    internal interface IHeidelpayProvider
    {
        /// <summary>
        /// Gets or sets the heidelpay client.
        /// </summary>
        /// <value>The heidelpay.</value>
        IHeidelpay Heidelpay { get; set; }
    }
}
