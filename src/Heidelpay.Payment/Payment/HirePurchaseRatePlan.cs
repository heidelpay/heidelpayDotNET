using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    /// <summary>
    /// HirePurchaseRatePlan
    /// </summary>
    public sealed class HirePurchaseRatePlan : PaymentTransactionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HirePurchaseRatePlan"/> class.
        /// </summary>
        [JsonConstructor]
        internal HirePurchaseRatePlan()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HirePurchaseRatePlan"/> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public HirePurchaseRatePlan(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }
    }
}
