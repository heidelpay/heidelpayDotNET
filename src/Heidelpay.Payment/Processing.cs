// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Processing.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class Processing.
    /// </summary>
    public class Processing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Processing"/> class.
        /// </summary>
        [JsonConstructor]
        internal Processing()
        {

        }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        [JsonProperty]
        public string UniqueId { get; internal set; }

        /// <summary>
        /// Gets the short identifier.
        /// </summary>
        /// <value>The short identifier.</value>
        [JsonProperty]
        public string ShortId { get; internal set; }
    }
}
