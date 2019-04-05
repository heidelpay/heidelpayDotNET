// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 03-14-2019
// ***********************************************************************
// <copyright file="RestClientError.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Heidelpay.Payment.Communication
{
    /// <summary>
    /// Class RestClientError.
    /// </summary>
    public class RestClientError
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the merchant message.
        /// </summary>
        /// <value>The merchant message.</value>
        public string MerchantMessage { get; set; }
        /// <summary>
        /// Gets or sets the customer message.
        /// </summary>
        /// <value>The customer message.</value>
        public string CustomerMessage { get; set; }
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>The customer.</value>
        public string Customer { get; set; }
    }
}
