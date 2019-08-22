// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified By : berghtho

// ***********************************************************************
// <copyright file="Cancel.cs" company="Heidelpay">
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
    /// Business object for Cancellations
    /// 
    /// Implements the <see cref="Heidelpay.Payment.TransactionBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.TransactionBase" />
    public sealed class Cancel : TransactionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cancel"/> class.
        /// </summary>
        [JsonConstructor]
        internal Cancel()
        {
        }

        /// <summary>
        /// Gets or sets the amount gross.
        /// </summary>
        /// <value>
        /// The amount gross.
        /// </value>
        public decimal? AmountGross { get; set; }

        /// <summary>
        /// Gets or sets the amount net.
        /// </summary>
        /// <value>
        /// The amount net.
        /// </value>
        public decimal? AmountNet { get; set; }

        /// <summary>
        /// Gets or sets the amount vat.
        /// </summary>
        /// <value>
        /// The amount vat.
        /// </value>
        public decimal? AmountVat { get; set; }
    }
}
