// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Shipment.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class Shipment.
    /// Implements the <see cref="Heidelpay.Payment.PaymentBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentBase" />
    public class Shipment : PaymentBase
    {
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public string InvoiceId { get; set; }

        /// <summary>
        /// Gets the payment.
        /// </summary>
        /// <value>The payment.</value>
        [JsonProperty]
        public Payment Payment { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Refund"/> class.
        /// </summary>
        [JsonConstructor]
        internal Shipment()
        {

        }

        /// <summary>Initializes a new instance of the <see cref="T:Heidelpay.Payment.Shipment"/> class.</summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        public Shipment(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        /// <value>The resources.</value>
        [JsonProperty]
        internal Resources Resources { get; set; } = new Resources();

        /// <summary>
        /// Gets the type URL.
        /// </summary>
        /// <value>The type URL.</value>
        public override string TypeUrl => "payments/<paymentId>/shipments";
    }
}
