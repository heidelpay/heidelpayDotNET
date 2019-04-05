// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 04-01-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Amount.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Heidelpay.Payment.Communication.Internal
{
    /// <summary>
    /// Class Amount.
    /// </summary>
    internal class Amount
    {
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public decimal Total { get; set; }
        /// <summary>
        /// Gets or sets the charged.
        /// </summary>
        /// <value>The charged.</value>
        public decimal Charged { get; set; }
        /// <summary>
        /// Gets or sets the canceled.
        /// </summary>
        /// <value>The canceled.</value>
        public decimal Canceled { get; set; }
        /// <summary>
        /// Gets or sets the remaining.
        /// </summary>
        /// <value>The remaining.</value>
        public decimal Remaining { get; set; }
    }
}
