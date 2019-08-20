// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-15-2019
// ***********************************************************************
// <copyright file="Authorization.cs" company="Heidelpay">
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
    /// 
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentTransactionBase" />
    public sealed class Payout : PaymentTransactionBase
    {
        [JsonConstructor]
        internal Payout()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Payout"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        internal Payout(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Payout"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client.</param>
        /// <param name="paymentType">Type of the payment.</param>
        public Payout(IHeidelpay heidelpayClient, IPaymentType paymentType)
            : base(heidelpayClient, paymentTypeId: paymentType.Id)
        {
        }
    }
}
