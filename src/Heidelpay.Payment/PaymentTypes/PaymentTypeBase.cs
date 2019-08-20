// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="PaymentTypeBase.cs" company="Heidelpay">
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

namespace Heidelpay.Payment.PaymentTypes
{
    /// <summary>
    /// Class PaymentTypeBase.
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IRestResource" />
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IHeidelpayProvider" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.Interfaces.IRestResource" />
    /// <seealso cref="Heidelpay.Payment.Interfaces.IHeidelpayProvider" />
    public abstract class PaymentTypeBase : IRestResource, IHeidelpayProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeBase"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public PaymentTypeBase(IHeidelpay heidelpayClient)
        {
            ((IHeidelpayProvider)this).Heidelpay = heidelpayClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeBase"/> class.
        /// </summary>
        public PaymentTypeBase()
        {
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PaymentTypeBase"/> is recurring.
        /// </summary>
        /// <value>
        ///   <c>true</c> if recurring; otherwise, <c>false</c>.
        /// </value>
        public bool Recurring { get; set; }
       
        /// <summary>
        /// Gets or sets the heidelpay client.
        /// </summary>
        /// <value>The heidelpay client.</value>
        IHeidelpay IHeidelpayProvider.Heidelpay { get; set; }

        /// <summary>
        /// Gets the heidelpay client.
        /// </summary>
        /// <value>The heidelpay client.</value>
        protected IHeidelpay Heidelpay { get => ((IHeidelpayProvider)this).Heidelpay; }
    }
}
