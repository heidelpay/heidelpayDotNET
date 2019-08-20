// ***********************************************************************
// Assembly         : Heidelpay.Payment

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

        /// <summary>
        /// Gets or sets the descriptor.
        /// </summary>
        /// <value>
        /// The descriptor.
        /// </value>
        public string Descriptor { get; set; }

        /// <summary>
        /// Gets or sets the bic.
        /// </summary>
        /// <value>
        /// The bic.
        /// </value>
        public string Bic { get; set; }

        /// <summary>
        /// Gets or sets the iban.
        /// </summary>
        /// <value>
        /// The iban.
        /// </value>
        public string Iban { get; set; }

        /// <summary>
        /// Gets or sets the holder.
        /// </summary>
        /// <value>
        /// The holder.
        /// </value>
        public string Holder { get; set; }

        /// <summary>
        /// Gets or sets the PDF link.
        /// </summary>
        /// <value>
        /// The PDF link.
        /// </value>
        public string PdfLink { get; set; }

        /// <summary>
        /// Gets or sets the external order identifier.
        /// </summary>
        /// <value>
        /// The external order identifier.
        /// </value>
        public string ExternalOrderId { get; set; }
    }
}
