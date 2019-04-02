// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Resources.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class Resources.
    /// </summary>
    public class Resources
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Resources"/> class.
        /// </summary>
        [JsonConstructor]
        internal Resources()
        {

        }

        /// <summary>
        /// Gets the type identifier.
        /// </summary>
        /// <value>The type identifier.</value>
        [JsonProperty]
        public string TypeId { get; internal set; }

        /// <summary>
        /// Gets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        [JsonProperty]
        public string CustomerId { get; internal set; }

        /// <summary>
        /// Gets the metadata identifier.
        /// </summary>
        /// <value>The metadata identifier.</value>
        [JsonProperty]
        public string MetadataId { get; internal set; }

        /// <summary>
        /// Gets the payment identifier.
        /// </summary>
        /// <value>The payment identifier.</value>
        [JsonProperty]
        public string PaymentId { get; internal set; }

        /// <summary>
        /// Gets the risk identifier.
        /// </summary>
        /// <value>The risk identifier.</value>
        [JsonProperty]
        public string RiskId { get; internal set; }

        /// <summary>
        /// Gets the basket identifier.
        /// </summary>
        /// <value>The basket identifier.</value>
        [JsonProperty]
        public string BasketId { get; internal set; }
    }
}
