// ***********************************************************************
// Assembly         : Heidelpay.Payment

// ***********************************************************************
// <copyright file="PaymentError.cs" company="Heidelpay">
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
