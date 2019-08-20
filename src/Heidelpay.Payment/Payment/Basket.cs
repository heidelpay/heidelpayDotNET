// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-15-2019
// ***********************************************************************
// <copyright file="Basket.cs" company="Heidelpay">
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
using System.Collections.Generic;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class Basket.
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IRestResource" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.Interfaces.IRestResource" />
    public class Basket : IRestResource
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the amount total.
        /// </summary>
        /// <value>The amount total.</value>
        public decimal AmountTotal { get; set; }

        public decimal AmountTotalVat { get; set; }

        /// <summary>
        /// Gets or sets the amount total discount.
        /// </summary>
        /// <value>The amount total discount.</value>
        public decimal AmountTotalDiscount { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        /// <value>The currency code.</value>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>The note.</value>
        public string Note { get; set; }

        /// <summary>
        /// The basket items
        /// </summary>
        readonly List<BasketItem> basketItems = new List<BasketItem>();
        /// <summary>
        /// Gets the basket items.
        /// </summary>
        /// <value>The basket items.</value>
        public IEnumerable<BasketItem> BasketItems { get => basketItems; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Basket"/> class.
        /// </summary>
        public Basket()
        {
        }

        /// <summary>
        /// Adds the basket item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void AddBasketItem(BasketItem item)
        {
            basketItems.Add(item);
        }
    }
}
