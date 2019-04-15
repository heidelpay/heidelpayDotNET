// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Message.cs" company="Heidelpay">
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
        [JsonConstructor]
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
