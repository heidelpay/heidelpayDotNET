// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="Charge.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Business object for Charge. Amount, currency and typeId are mandatory parameter to 
    /// execute an Charge.
    /// 
    /// The returnUrl is mandatory in case of redirectPayments like Sofort, Paypal, Giropay, Creditcard 3DS
    /// 
    /// Implements the <see cref="Heidelpay.Payment.PaymentTransactionBase" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.PaymentTransactionBase" />
    public class Charge : PaymentTransactionBase
    {
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public string InvoiceId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Charge"/> class.
        /// </summary>
        [JsonConstructor]
        internal Charge()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Charge"/> class.
        /// </summary>
        /// <param name="chargeablePayment">The chargeable payment.</param>
        public Charge(IChargeablePaymentType chargeablePayment)
            : base(chargeablePayment.Heidelpay, paymentTypeId: chargeablePayment.Id)
        {
            if (chargeablePayment is IProvide3DS threeDSprovider)
            {
                Card3ds = threeDSprovider.ThreeDs;
            }
        }

        /// <summary>
        /// cancel as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> CancelAsync(decimal? amount = null)
        {
            return await Heidelpay.CancelChargeAsync(Payment.Id, Id, amount);
        }

        /// <summary>
        /// Gets the cancel.
        /// </summary>
        /// <param name="cancelId">The cancel identifier.</param>
        /// <returns>Cancel.</returns>
        public Cancel GetCancel(string cancelId)
        {
            return CancelList?.FirstOrDefault(x => string.Equals(x.Id, cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the type URL.
        /// </summary>
        /// <value>The type URL.</value>
        public override string TypeUrl => "payments/<paymentId>/charges";
    }

    /// <summary>
    /// Enum Status
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// The success
        /// </summary>
        Success,
        /// <summary>
        /// The pending
        /// </summary>
        Pending,
        /// <summary>
        /// The error
        /// </summary>
        Error,
        /// <summary>
        /// The undefined
        /// </summary>
        Undefined,
    }
}
