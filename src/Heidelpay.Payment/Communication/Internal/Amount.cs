﻿// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="Amount.cs" company="Heidelpay">
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

namespace Heidelpay.Payment.Communication.Internal
{
    /// <summary>
    /// Class Amount.
    /// </summary>
    internal class Amount
    {
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public decimal Total { get; set; }
        /// <summary>
        /// Gets or sets the charged.
        /// </summary>
        /// <value>The charged.</value>
        public decimal Charged { get; set; }
        /// <summary>
        /// Gets or sets the canceled.
        /// </summary>
        /// <value>The canceled.</value>
        public decimal Canceled { get; set; }
        /// <summary>
        /// Gets or sets the remaining.
        /// </summary>
        /// <value>The remaining.</value>
        public decimal Remaining { get; set; }
    }
}
