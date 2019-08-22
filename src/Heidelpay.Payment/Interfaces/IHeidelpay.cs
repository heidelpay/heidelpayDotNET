// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="IHeidelpay.cs" company="Heidelpay">
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

using Heidelpay.Payment.PaymentTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Interfaces
{
    /// <summary>
    /// Interface IHeidelpay
    /// </summary>
    public interface IHeidelpay
    {
        /// <summary>
        /// Authorizes as an asynchronous operation.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        Task<Authorization> AuthorizeAsync(Authorization authorization);

        /// <summary>
        /// Authorizes as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        Task<Authorization> AuthorizeAsync(decimal amount, string currency, IAuthorizedPaymentType paymentType);

        /// <summary>
        /// Authorizes as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="card3ds">The card3ds.</param>
        /// <returns>
        /// Task&lt;Authorization&gt;.
        /// </returns>
        Task<Authorization> AuthorizeAsync(decimal amount, string currency, IAuthorizedPaymentType paymentType, Uri returnUrl, Customer customer = null, bool? card3ds = null);

        /// <summary>
        /// Authorizes the asynchronous.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="basket">The basket.</param>
        /// <param name="effectiveInterestRate">The effective interest rate.</param>
        /// <returns></returns>
        Task<Authorization> AuthorizeAsync(decimal amount, string currency, IAuthorizedPaymentType paymentType, Uri returnUrl, Customer customer, Basket basket, decimal? effectiveInterestRate = null);

        /// <summary>
        /// Authorizes as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        Task<Authorization> AuthorizeAsync(decimal amount, string currency, IAuthorizedPaymentType paymentType, Uri returnUrl, string customerId);

        /// <summary>
        /// Authorizes as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="authorizedPaymentTypeId">The authorized payment type identifier.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="card3ds">The card3ds.</param>
        /// <returns>
        /// Task&lt;Authorization&gt;.
        /// </returns>
        Task<Authorization> AuthorizeAsync(decimal amount, string currency, string authorizedPaymentTypeId, Uri returnUrl = null, string customerId = null, bool? card3ds = null);

        /// <summary>
        /// Authorizes the asynchronous.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="authorizedPaymentTypeId">The authorized payment type identifier.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="basketId">The basket identifier.</param>
        /// <param name="effectiveInterestRate">The effective interest rate.</param>
        /// <returns></returns>
        Task<Authorization> AuthorizeAsync(decimal amount, string currency, string authorizedPaymentTypeId, Uri returnUrl, string customerId, string basketId, decimal? effectiveInterestRate = null);

        /// <summary>
        /// Cancels the authorization asynchronous.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        Task<Cancel> CancelAuthorizationAsync(string paymentId, decimal? amount = null);

        /// <summary>
        /// Cancels the charge as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        Task<Cancel> CancelChargeAsync(string paymentId, string chargeId, decimal? amount = null);

        /// <summary>
        /// Charges as an asynchronous operation.
        /// </summary>
        /// <param name="charge">The charge.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        Task<Charge> ChargeAsync(Charge charge);

        /// <summary>
        /// Charges as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="card3ds">if set to <c>true</c> [card3ds].</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        Task<Charge> ChargeAsync(decimal amount, string currency, IChargeablePaymentType paymentType, bool? card3ds = null);

        /// <summary>
        /// Charges as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="card3ds">if set to <c>true</c> [card3ds].</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        Task<Charge> ChargeAsync(decimal amount, string currency, IChargeablePaymentType paymentType, Uri returnUrl, Customer customer = null, bool? card3ds = null);
        /// <summary>
        /// Charges as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="basket">The basket.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <param name="card3ds">if set to <c>true</c> [card3ds].</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        Task<Charge> ChargeAsync(decimal amount, string currency, IChargeablePaymentType paymentType, Uri returnUrl, Customer customer, Basket basket, string invoiceId = null, bool? card3ds = null);

        /// <summary>
        /// Charges as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="card3ds">if set to <c>true</c> [card3ds].</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        Task<Charge> ChargeAsync(decimal amount, string currency, IChargeablePaymentType paymentType, Uri returnUrl, string customerId, bool? card3ds = null);

        /// <summary>
        /// Charges as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="chargeablePaymentTypeId">The chargeable payment type identifier.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="card3ds">if set to <c>true</c> [card3ds].</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        Task<Charge> ChargeAsync(decimal amount, string currency, string chargeablePaymentTypeId, Uri returnUrl = null, string customerId = null, bool? card3ds = null);

        /// <summary>
        /// Charges the authorization as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        Task<Charge> ChargeAuthorizationAsync(string paymentId, decimal? amount = null);

        /// <summary>
        /// Creates the customer as an asynchronous operation.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        Task<Customer> CreateCustomerAsync(Customer customer);

        /// <summary>
        /// Creates the payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentBase">The type of the t payment base.</typeparam>
        /// <param name="config">The configuration.</param>
        /// <returns>Task&lt;TPaymentBase&gt;.</returns>
        Task<TPaymentBase> CreatePaymentTypeAsync<TPaymentBase>(Action<TPaymentBase> config = null) where TPaymentBase : PaymentTypeBase;

        /// <summary>
        /// Creates the payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentBase">The type of the payment base.</typeparam>
        /// <param name="paymentTypeInstance">The payment type instance.</param>
        /// <returns></returns>
        Task<TPaymentBase> CreatePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentTypeInstance) where TPaymentBase : PaymentTypeBase;

        /// <summary>
        /// Updates the payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentBase">The type of the payment base.</typeparam>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        Task<TPaymentBase> UpdatePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentType) where TPaymentBase : PaymentTypeBase;

        /// <summary>
        /// Deletes the customer as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        Task DeleteCustomerAsync(string id);

        /// <summary>
        /// Fetches the authorization as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        Task<Authorization> FetchAuthorizationAsync(string paymentId);

        /// <summary>
        /// Fetches the basket as an asynchronous operation.
        /// </summary>
        /// <param name="basketId">The basket identifier.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        Task<Basket> FetchBasketAsync(string basketId);

        /// <summary>
        /// Fetches the charge as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        Task<Charge> FetchChargeAsync(string paymentId, string chargeId);

        /// <summary>
        /// Fetches the payout as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        Task<Payout> FetchPayoutAsync(string paymentId, string chargeId);

        /// <summary>
        /// Fetches the cancel as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="cancelId">The cancel identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        Task<Cancel> FetchCancelAsync(string paymentId, string cancelId);

        /// <summary>
        /// Fetches the cancel as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <param name="cancelId">The cancel identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        Task<Cancel> FetchCancelAsync(string paymentId, string chargeId, string cancelId);

        /// <summary>
        /// Fetches the customer as an asynchronous operation.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        Task<Customer> FetchCustomerAsync(string customerId);

        /// <summary>
        /// Fetches the meta data as an asynchronous operation.
        /// </summary>
        /// <param name="metaDataId">The meta data identifier.</param>
        /// <returns>Task&lt;MetaData&gt;.</returns>
        Task<MetaData> FetchMetaDataAsync(string metaDataId);

        /// <summary>
        /// Fetches the payment as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Payment&gt;.</returns>
        Task<Payment> FetchPaymentAsync(string paymentId);

        /// <summary>
        /// Fetches the payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentType">The type of the t payment type.</typeparam>
        /// <param name="typeId">The type identifier.</param>
        /// <returns>Task&lt;TPaymentType&gt;.</returns>
        Task<TPaymentType> FetchPaymentTypeAsync<TPaymentType>(string typeId) where TPaymentType : PaymentTypeBase;

        /// <summary>
        /// Shipments as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>Task&lt;Shipment&gt;.</returns>
        Task<Shipment> ShipmentAsync(string paymentId, string invoiceId = null);

        /// <summary>
        /// Updates the customer as an asynchronous operation.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        Task<Customer> UpdateCustomerAsync(Customer customer);

        /// <summary>
        /// Creates the basket as an asynchronous operation.
        /// </summary>
        /// <param name="basket">The basket.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        Task<Basket> CreateBasketAsync(Basket basket);

        /// <summary>
        /// Creates the metadata as an asynchronous operation.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>Task&lt;MetaDatas&gt;.</returns>
        Task<MetaData> CreateMetadataAsync(MetaData metadata);

        /// <summary>
        /// Updates the basket as an asynchronous operation.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        Task<Basket> UpdateBasketAsync(Basket customer);

        /// <summary>
        /// Payout as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        Task<Payout> PayoutAsync(decimal amount, string currency, IChargeablePaymentType paymentType, Uri returnUrl);

        /// <summary>
        /// Payout as an asynchronous operation.
        /// </summary>
        /// <param name="payout">The payout.</param>
        /// <returns></returns>
        Task<Payout> PayoutAsync(Payout payout);

        /// <summary>
        /// Recurrings as an asynchronous operation.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="metadataId">The metadata identifier.</param>
        /// <returns></returns>
        Task<Recurring> RecurringAsync(IPaymentType paymentType, Uri returnUrl, string customerId = null, string metadataId = null);

        /// <summary>
        /// Paypages as an asynchronous operation.
        /// </summary>
        /// <param name="paypage">The paypage.</param>
        /// <returns></returns>
        Task<Paypage> PaypageAsync(Paypage paypage);


        /// <summary>
        /// Hires the purchase rates as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="effectiveInterestRate">The effective interest rate.</param>
        /// <param name="orderDate">The order date.</param>
        /// <returns></returns>
        Task<IEnumerable<HirePurchaseRatePlan>> HirePurchaseRatesAsync(decimal amount, string currency, decimal effectiveInterestRate, DateTime orderDate);
    }
}