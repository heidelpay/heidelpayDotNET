// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 03-14-2019
// ***********************************************************************
// <copyright file="BasketItem.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Heidelpay.Payment
{
    /// <summary>
    /// Class BasketItem.
    /// </summary>
    public class BasketItem
    {
        /// <summary>
        /// Gets or sets the basket item reference identifier.
        /// </summary>
        /// <value>The basket item reference identifier.</value>
        public string BasketItemReferenceId { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the vat.
        /// </summary>
        /// <value>The vat.</value>
        public decimal Vat { get; set; }

        /// <summary>
        /// Gets or sets the amount discount.
        /// </summary>
        /// <value>The amount discount.</value>
        public decimal AmountDiscount { get; set; }

        /// <summary>
        /// Gets or sets the amount gross.
        /// </summary>
        /// <value>The amount gross.</value>
        public decimal AmountGross { get; set; }

        /// <summary>
        /// Gets or sets the amount vat.
        /// </summary>
        /// <value>The amount vat.</value>
        public decimal AmountVat { get; set; }

        /// <summary>
        /// Gets or sets the amount per unit.
        /// </summary>
        /// <value>The amount per unit.</value>
        public decimal AmountPerUnit { get; set; }

        /// <summary>
        /// Gets or sets the amount net.
        /// </summary>
        /// <value>The amount net.</value>
        public decimal AmountNet { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>The unit.</value>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
    }
}
