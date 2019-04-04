// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="MetaData.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Communication.Converter;
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class MetaData.
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IRestResource" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.Interfaces.IRestResource" />
    public class MetaData : IRestResource
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the metadata map.
        /// </summary>
        /// <value>The metadata map.</value>
        [JsonProperty]
        public IDictionary<string, string> MetadataMap { get; internal set; } = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaData"/> class.
        /// </summary>
        /// <param name="sorted">if set to <c>true</c> [sorted].</param>
        public MetaData(bool sorted = false)
        {
            if(sorted)
            {
                MetadataMap = new SortedDictionary<string, string>();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public string this[string key]
        {
            get
            {
                return MetadataMap[key];
            }
            set
            {
                MetadataMap[key] = value;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                return MetadataMap.Count;
            }
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(string key)
        {
            return MetadataMap.ContainsKey(key);
        }

        /// <summary>
        /// Gets the type URL.
        /// </summary>
        /// <value>The type URL.</value>
        public string TypeUrl => "metadata";
    }
}
