// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 03-14-2019
// ***********************************************************************
// <copyright file="BasketItem.cs" company="Heidelpay">
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

        /// <summary>
        /// Gets or sets the sub title.
        /// </summary>
        /// <value>The sub title.</value>
        public string SubTitle { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; set; }
    }
}
