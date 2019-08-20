// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-04-2019
// ***********************************************************************
// <copyright file="Payment.cs" company="Heidelpay">
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

using Heidelpay.Payment.Communication.Internal;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.PaymentTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Business object for a payment. A payment is the object that combines several
    /// requests over the lifetime of a payment transaction. This means that one payment
    /// always relates to one offer from the merchant.
    /// Implements the <see cref="PaymentBase" />
    /// </summary>
    /// <seealso cref="PaymentBase" />
    public class Payment : PaymentBase
    {
        /// <summary>
        /// Gets or sets the state value.
        /// </summary>
        /// <value>The state value.</value>
        [JsonProperty(PropertyName = "State")]
        internal StateValue StateValue { get; set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        [JsonIgnore]
        public State State
        {
            get
            {
                return (State)StateValue.Id;
            }
        }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        public string OrderId { get; set; }

        /// <summary>
        /// Gets the authorization.
        /// </summary>
        /// <value>The authorization.</value>
        public Authorization Authorization { get; internal set; }

        /// <summary>
        /// Gets the charges list.
        /// </summary>
        /// <value>The charges list.</value>
        public IEnumerable<Charge> ChargesList { get; internal set; } = Enumerable.Empty<Charge>();

        /// <summary>
        /// Gets the payout list.
        /// </summary>
        /// <value>The payout list.</value>
        public IEnumerable<Payout> PayoutList { get; internal set; } = Enumerable.Empty<Payout>();

        /// <summary>
        /// Gets the cancel list.
        /// </summary>
        /// <value>The cancel list.</value>
        public IEnumerable<Cancel> CancelList { get; internal set; } = Enumerable.Empty<Cancel>();

        /// <summary>
        /// Gets the type identifier.
        /// </summary>
        /// <value>The type identifier.</value>
        [JsonIgnore]
        public string TypeId
        {
            get
            {
                return Resources?.TypeId;
            }
        }

        /// <summary>
        /// Gets the basket identifier.
        /// </summary>
        /// <value>The basket identifier.</value>
        [JsonIgnore]
        public string BasketId
        {
            get
            {
                return Resources?.BasketId;
            }
        }

        /// <summary>
        /// Gets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        [JsonIgnore]
        public string CustomerId
        {
            get
            {
                return Resources?.CustomerId;
            }
        }

        /// <summary>
        /// Gets the metadata identifier.
        /// </summary>
        /// <value>The metadata identifier.</value>
        [JsonIgnore]
        public string MetadataId
        {
            get
            {
                return Resources?.MetadataId;
            }
        }

        /// <summary>
        /// Gets the risk identifier.
        /// </summary>
        /// <value>The risk identifier.</value>
        [JsonIgnore]
        public string RiskId
        {
            get
            {
                return Resources?.RiskId;
            }
        }

        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        /// <value>The resources.</value>
        [JsonProperty]
        internal Resources Resources { get; set; } = new Resources();

        /// <summary>
        /// Gets the amount total.
        /// </summary>
        /// <value>The amount total.</value>
        public decimal? AmountTotal { get => Amount?.Total; }

        /// <summary>
        /// Gets the amount charged.
        /// </summary>
        /// <value>The amount charged.</value>
        public decimal? AmountCharged { get => Amount?.Charged; }

        /// <summary>
        /// Gets the amount canceled.
        /// </summary>
        /// <value>The amount canceled.</value>
        public decimal? AmountCanceled { get => Amount?.Canceled; }

        /// <summary>
        /// Gets the amount remaining.
        /// </summary>
        /// <value>The amount remaining.</value>
        public decimal? AmountRemaining { get => Amount?.Remaining; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Payment" /> class.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        public Payment(PaymentTypeBase paymentType)
            : this(((IHeidelpayProvider)paymentType).Heidelpay)
        {
            Resources.TypeId = paymentType.Id;
        }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        [JsonProperty]
        internal Amount Amount { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>The transactions.</value>
        [JsonProperty]
        internal IEnumerable<Transaction> Transactions { get; set; } = Enumerable.Empty<Transaction>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Heidelpay.Payment.Payment" /> class.
        /// </summary>
        /// <param name="heidelpayClient">The heidelpay client instance.</param>
        internal Payment(IHeidelpay heidelpayClient)
            : base(heidelpayClient)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heidelpay.Payment.Payment" /> class.
        /// </summary>
        [JsonConstructor]
        internal Payment()
        {

        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(decimal? amount = null)
        {
            return await Heidelpay.ChargeAuthorizationAsync(Id, amount);
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(decimal amount, string currency, Uri returnUrl = null)
        {
            return await Heidelpay.ChargeAsync(amount, currency, Resources.TypeId, returnUrl);
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(decimal amount, string currency, Uri returnUrl, string customerId)
        {
            return await Heidelpay.ChargeAsync(amount, currency, Resources.TypeId, returnUrl, customerId);
        }

        /// <summary>
        /// authorize as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, Uri returnUrl = null, string customerId = null)
        {
            return await Heidelpay.AuthorizeAsync(amount, currency, Resources.TypeId, returnUrl, customerId);
        }

        /// <summary>
        /// authorize as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, Uri returnUrl, Customer customer)
        {
            return await Heidelpay.AuthorizeAsync(amount, currency, Resources.TypeId, returnUrl, customer.Id);
        }

        /// <summary>
        /// cancel as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> CancelAsync(decimal? amount = null)
        {
            Check.ThrowIfTrue(Authorization == null, 
                merchantMessage: "Cancel is only possible for an Authorization", 
                customerMessage: "Payment cancelation not possible");

            return await Authorization.CancelAsync(amount);
        }

        /// <summary>
        /// Gets the charge.
        /// </summary>
        /// <param name="chargeId">The charge identifier.</param>
        /// <returns>Charge.</returns>
        public Charge GetCharge(string chargeId)
        {
            return ChargesList?.FirstOrDefault(x => string.Equals(x.Id, chargeId, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the payout.
        /// </summary>
        /// <param name="payoutId">The payout identifier.</param>
        /// <returns></returns>
        public Payout GetPayout(string payoutId)
        {
            return PayoutList?.FirstOrDefault(x => string.Equals(x.Id, payoutId, StringComparison.InvariantCultureIgnoreCase));
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

        Customer customer;
        /// <summary>
        /// fetch customer as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> FetchCustomerAsync()
        {
            if (customer == null && IsNotEmpty(Resources?.CustomerId))
                customer = await Heidelpay.FetchCustomerAsync(Resources?.CustomerId);

            return customer;
        }

        PaymentTypeBase paymentType;
        /// <summary>
        /// fetch payment type as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;PaymentTypeBase&gt;.</returns>
        public async Task<PaymentTypeBase> FetchPaymentTypeAsync()
        {
            if (paymentType == null && IsNotEmpty(Resources?.TypeId))
                paymentType = await Heidelpay.FetchPaymentTypeAsync<PaymentTypeBase>(Resources?.TypeId);

            return paymentType;
        }

        MetaData metaData;
        /// <summary>
        /// fetch meta data as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;MetaData&gt;.</returns>
        public async Task<MetaData> FetchMetaDataAsync()
        {
            if (metaData == null && IsNotEmpty(Resources?.MetadataId))
                metaData = await Heidelpay.FetchMetaDataAsync(Resources?.MetadataId);

            return metaData;
        }

        Basket basket;
        /// <summary>
        /// fetch basket as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> FetchBasketAsync()
        {
            if (basket == null && IsNotEmpty(Resources?.BasketId))
                basket = await Heidelpay.FetchBasketAsync(Resources?.BasketId);

            return basket;
        }

        /// <summary>
        /// The is not empty
        /// </summary>
        static readonly Func<string, bool> IsNotEmpty = CoreExtensions.IsNotEmpty;
    }

    /// <summary>
    /// Enum State
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Payment pending
        /// </summary>
        Pending = 0,
        /// <summary>
        /// Payment completed
        /// </summary>
        Completed = 1,
        /// <summary>
        /// Payment canceled
        /// </summary>
        Canceled = 2,
        /// <summary>
        /// Payment partly completed
        /// </summary>
        Partly = 3,
        /// <summary>
        /// Payment in review
        /// </summary>
        Payment_review = 4,
        /// <summary>
        /// Payment chargeback
        /// </summary>
        Chargeback = 5,
    }

    /// <summary>
    /// Class StateValue.
    /// </summary>
    internal class StateValue
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
    }
}
