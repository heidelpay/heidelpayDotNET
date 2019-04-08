// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="Basket.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
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
