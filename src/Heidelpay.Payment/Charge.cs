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
using System.Collections.Generic;
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
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }
        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        /// <value>The return URL.</value>
        public Uri ReturnUrl { get; set; }
        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>The redirect URL.</value>
        public Uri RedirectUrl { get; set; }
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        public string OrderId { get; set; }
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public string InvoiceId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Charge"/> is card3ds.
        /// </summary>
        /// <value><c>null</c> if [card3ds] contains no value, <c>true</c> if [card3ds]; otherwise, <c>false</c>.</value>
        public bool? Card3ds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is success.
        /// </summary>
        /// <value><c>true</c> if this instance is success; otherwise, <c>false</c>.</value>
        [JsonProperty]
        internal bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pending.
        /// </summary>
        /// <value><c>true</c> if this instance is pending; otherwise, <c>false</c>.</value>
        [JsonProperty]
        internal bool IsPending { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is error.
        /// </summary>
        /// <value><c>true</c> if this instance is error; otherwise, <c>false</c>.</value>
        [JsonProperty]
        internal bool IsError { get; set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonIgnore]
        public Status Status
        {
            get
            {
                if (IsSuccess)
                    return Status.Success;

                if (IsPending)
                    return Status.Pending;

                if (IsError)
                    return Status.Error;

                return Status.Undefined;
            }
        }

        /// <summary>
        /// Gets or sets the cancel list.
        /// </summary>
        /// <value>The cancel list.</value>
        public IEnumerable<Cancel> CancelList { get; set; } = Enumerable.Empty<Cancel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Charge"/> class.
        /// </summary>
        /// <param name="chargeablePayment">The chargeable payment.</param>
        public Charge(IChargeablePaymentType chargeablePayment)
            : this(chargeablePayment.Heidelpay)
        {
            Resources.TypeId = chargeablePayment.Id;
        }

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
        /// <param name="heidelpayClient">The heidelpay.</param>
        internal Charge(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {
        }

        /// <summary>
        /// Gets or sets the processing.
        /// </summary>
        /// <value>The processing.</value>
        [JsonProperty]
        internal Processing Processing { get; set; } = new Processing();

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
