using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    /// <summary>
    /// HirePurchaseRate
    /// </summary>
    public sealed class HirePurchaseRate
    {
        /// <summary>
        /// Gets or sets the amount of repayment.
        /// </summary>
        /// <value>
        /// The amount of repayment.
        /// </value>
        public decimal? AmountOfRepayment { get; set; }

        /// <summary>
        /// Gets or sets the rate.
        /// </summary>
        /// <value>
        /// The rate.
        /// </value>
        public decimal? Rate { get; set; }

        /// <summary>
        /// Gets or sets the total remaining amount.
        /// </summary>
        /// <value>
        /// The total remaining amount.
        /// </value>
        public decimal? TotalRemainingAmount { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the index of the rate.
        /// </summary>
        /// <value>
        /// The index of the rate.
        /// </value>
        public int? RateIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HirePurchaseRate"/> is ultimo.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ultimo; otherwise, <c>false</c>.
        /// </value>
        public bool? Ultimo { get; set; }
    }
}
