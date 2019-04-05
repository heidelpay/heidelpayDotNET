// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 04-01-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Message.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace Heidelpay.Payment.Communication.Internal
{
    /// <summary>
    /// Class Message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        internal Message()
        {

        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code.</value>
        [JsonProperty]
        public string Code { get; internal set; }

        /// <summary>
        /// Gets the customer.
        /// </summary>
        /// <value>The customer.</value>
        [JsonProperty]
        public string Customer { get; internal set; }

        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <value>The merchant.</value>
        [JsonProperty]
        public string Merchant { get; internal set; }
    }
}
