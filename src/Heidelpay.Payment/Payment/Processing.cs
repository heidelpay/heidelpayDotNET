// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Processing.cs" company="Heidelpay">
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
