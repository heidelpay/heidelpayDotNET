// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Refund.cs" company="Heidelpay">
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

using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class Refund.
    /// Implements the <see cref="Heidelpay.Payment.PaymentBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentBase" />
    public sealed class Refund : PaymentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Refund"/> class.
        /// </summary>
        [JsonConstructor]
        internal Refund()
        {

        }

        /// <summary>Initializes a new instance of the <see cref="T:Heidelpay.Payment.Refund"/> class.</summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public Refund(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }
    }
}
