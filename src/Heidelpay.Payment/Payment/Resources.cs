﻿// ***********************************************************************
// Assembly         : Heidelpay.Payment

// ***********************************************************************
// <copyright file="Resources.cs" company="Heidelpay">
//     Copyright (c) 2019 Heidelpay GmbH. All rights reserved.
// </copyright>
// ***********************************************************************
// Licensed under the Apache License, Version 2.0 (the “License”);
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an “AS IS” BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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
