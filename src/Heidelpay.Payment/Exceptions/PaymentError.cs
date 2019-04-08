// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="PaymentError.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class PaymentError.
    /// </summary>
    public class PaymentError
    {
        /// <summary>
        /// Gets the merchant message.
        /// </summary>
        /// <value>The merchant message.</value>
        [JsonProperty]
        public string MerchantMessage { get; internal set; }

        /// <summary>
        /// Gets the customer message.
        /// </summary>
        /// <value>The customer message.</value>
        [JsonProperty]
        public string CustomerMessage { get; internal set; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code.</value>
        [JsonProperty]
        public string Code { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentError"/> class.
        /// </summary>
        [JsonConstructor]
        internal PaymentError()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentError"/> class.
        /// </summary>
        /// <param name="merchantMessage">The merchant message.</param>
        /// <param name="customerMessage">The customer message.</param>
        /// <param name="code">The code.</param>
        public PaymentError(string merchantMessage, string customerMessage, string code)
        {
            MerchantMessage = merchantMessage;
            CustomerMessage = customerMessage;
            Code = code;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{{merchantMessage: {MerchantMessage}, customerMessage: {CustomerMessage}, code: {Code}}}";
        }
    }
}
