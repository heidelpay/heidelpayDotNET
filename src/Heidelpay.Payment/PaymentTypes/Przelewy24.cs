﻿// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="Przelewy24.cs" company="Heidelpay">
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

namespace Heidelpay.Payment.PaymentTypes
{
    /// <summary>
    /// Class Przelewy24. This class cannot be inherited.
    /// Implements the <see cref="Heidelpay.Payment.PaymentTypes.PaymentTypeBase" />
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IChargeablePaymentType" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentTypes.PaymentTypeBase" />
    /// <seealso cref="Heidelpay.Payment.Interfaces.IChargeablePaymentType" />
    public sealed class Przelewy24 : PaymentTypeBase, IChargeablePaymentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Przelewy24"/> class.
        /// </summary>
        [JsonConstructor]
        internal Przelewy24()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Przelewy24"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public Przelewy24(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        /// <summary>
        /// Gets the heidelpay client.
        /// </summary>
        /// <value>The heidelpay client.</value>
        IHeidelpay IChargeablePaymentType.Heidelpay => Heidelpay;
    }
}
