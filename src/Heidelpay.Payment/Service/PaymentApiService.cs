// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="PaymentService.cs" company="Heidelpay">
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Service
{
    /// <summary>
    /// Class PaymentService. This class cannot be inherited.
    /// </summary>
    internal sealed class PaymentApiService : ApiServiceBase
    {
        private static readonly string TRANSACTION_TYPE_AUTHORIZATION = "authorize";
        private static readonly string TRANSACTION_TYPE_CHARGE = "charge";
        private static readonly string TRANSACTION_TYPE_PAYOUT = "payout";
        private static readonly string TRANSACTION_TYPE_CANCEL_AUTHORIZE = "cancel-authorize";
        private static readonly string TRANSACTION_TYPE_CANCEL_CHARGE = "cancel-charge";

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentApiService"/> class.
        /// </summary>
        /// <param name="heidelpay">The heidelpay client instance.</param>
        public PaymentApiService(HeidelpayClient heidelpay)
            : base(heidelpay)
        {
        }

        /// <summary>
        /// Create payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentBase">The type of the t payment base.</typeparam>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns>Task&lt;TPaymentBase&gt;.</returns>
        public async Task<TPaymentBase> CreatePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentType)
            where TPaymentBase : PaymentTypeBase
        {
            return await ApiPostAsync(paymentType);
        }

        /// <summary>
        /// Creates the payment type asynchronous.
        /// </summary>
        /// <param name="plan">The plan.</param>
        /// <returns></returns>
        public async Task<HirePurchaseRatePlan> CreatePaymentTypeAsync(HirePurchaseRatePlan plan)
        {
            return await ApiPostAsync(plan, uri: BuildUri("types/hire-purchase-direct-debit", null), getAfterPost: false);
        }

        /// <summary>
        /// create basket as an asynchronous operation.
        /// </summary>
        /// <param name="basket">The basket.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> CreateBasketAsync(Basket basket)
        {
            Check.ThrowIfNull(basket, nameof(basket));

            return await ApiPostAsync(basket);
        }

        /// <summary>
        /// Create meta data as an asynchronous operation.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>Task&lt;MetaData&gt;.</returns>
        public async Task<MetaData> CreateMetaDataAsync(MetaData metadata)
        {
            Check.ThrowIfNull(metadata, nameof(metadata));

            var created = await Heidelpay.RestClient.HttpPostAsync<MetaData>(BuildUri<MetaData>(), metadata.MetadataMap);
            created.MetadataMap = await Heidelpay.RestClient.HttpGetAsync<Dictionary<string,string>>(BuildUri<MetaData>(created.Id));
            return created;
        }

        /// <summary>
        /// Update basket as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="basket">The basket.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> UpdateBasketAsync(string id, Basket basket)
        {
            Check.ThrowIfNullOrEmpty(id, nameof(id));
            Check.ThrowIfNull(basket, nameof(basket));

            return await ApiPutAsync(id, basket, getAfterPut: true);
        }

        /// <summary>
        /// Create customer as an asynchronous operation.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            Check.ThrowIfNull(customer, nameof(customer));

            return await ApiPostAsync(customer, getAfterPost: true);
        }

        /// <summary>
        /// Update customer as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> UpdateCustomerAsync(string id, Customer customer)
        {
            Check.ThrowIfNullOrEmpty(id, nameof(id));
            Check.ThrowIfNull(customer, nameof(customer));

            return await ApiPutAsync(id, customer, getAfterPut: true);
        }

        /// <summary>
        /// Delete customer as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        public async Task DeleteCustomerAsync(string id)
        {
            Check.ThrowIfNullOrEmpty(id, nameof(id));

            await ApiDeleteAsync<Customer>(id);
        }

        /// <summary>
        /// Charge as an asynchronous operation.
        /// </summary>
        /// <param name="charge">The charge.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(Charge charge)
        {
            Check.ThrowIfNull(charge, nameof(charge));

            var result = await ApiPostAsync(charge, getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// Authorize as an asynchronous operation.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(Authorization authorization)
        {
            Check.ThrowIfNull(authorization, nameof(authorization));

            var result = await ApiPostAsync(authorization, getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// Fetch payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentBase">The type of the t payment base.</typeparam>
        /// <param name="paymentTypeId">The payment type identifier.</param>
        /// <returns>Task&lt;TPaymentBase&gt;.</returns>
        public async Task<TPaymentBase> FetchPaymentTypeAsync<TPaymentBase>(string paymentTypeId)
            where TPaymentBase : PaymentTypeBase
        {
            Check.ThrowIfNullOrEmpty(paymentTypeId, nameof(paymentTypeId));
            var paymentType = Registry.ResolvePaymentType(paymentTypeId);

            return await ApiGetAsync(paymentType, paymentTypeId) as TPaymentBase;
        }

        /// <summary>
        /// Fetch authorization as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> FetchAuthorizationAsync(string paymentId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            var payment = await FetchPaymentAsync(paymentId);

            return payment?.Authorization;
        }

        /// <summary>
        /// Fetch charge as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> FetchChargeAsync(string paymentId, string chargeId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            Check.ThrowIfNullOrEmpty(chargeId, nameof(chargeId));

            var payment = await FetchPaymentAsync(paymentId);

            return payment.GetCharge(chargeId);
        }

        /// <summary>
        /// Fetch payout as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="payoutId">The charge identifier.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Payout> FetchPayoutAsync(string paymentId, string payoutId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            Check.ThrowIfNullOrEmpty(payoutId, nameof(payoutId));

            var payment = await FetchPaymentAsync(paymentId);

            return payment.GetPayout(payoutId);
        }

        /// <summary>
        /// Fetch cancel as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="cancelId">The cancel identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> FetchCancelAsync(string paymentId, string cancelId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            Check.ThrowIfNullOrEmpty(cancelId, nameof(cancelId));

            var payment = await FetchPaymentAsync(paymentId);

            return payment.CancelList.FirstOrDefault(x => x.Id.Equals(cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Fetch cancel as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <param name="cancelId">The cancel identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> FetchCancelAsync(string paymentId, string chargeId, string cancelId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            Check.ThrowIfNullOrEmpty(cancelId, nameof(cancelId));
            Check.ThrowIfNullOrEmpty(chargeId, nameof(chargeId));

            var payment = await FetchPaymentAsync(paymentId);

            return payment?
                .ChargesList?
                .FirstOrDefault(x => x.Id.Equals(chargeId, StringComparison.InvariantCultureIgnoreCase))?
                .CancelList
                .FirstOrDefault(x => x.Id.Equals(cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Fetch payment as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Payment&gt;.</returns>
        public async Task<Payment> FetchPaymentAsync(string paymentId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            var payment = await ApiGetAsync<Payment>(paymentId);

            return await PostProcessPayment(payment);
        }

        /// <summary>
        /// Fetch basket as an asynchronous operation.
        /// </summary>
        /// <param name="basketId">The basket identifier.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> FetchBasketAsync(string basketId)
        {
            Check.ThrowIfNullOrEmpty(basketId, nameof(basketId));

            return await ApiGetAsync<Basket>(basketId);
        }

        /// <summary>
        /// Fetch customer as an asynchronous operation.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> FetchCustomerAsync(string customerId)
        {
            Check.ThrowIfNullOrEmpty(customerId, nameof(customerId));

            return await ApiGetAsync<Customer>(customerId);
        }

        /// <summary>
        /// Fetch meta data as an asynchronous operation.
        /// </summary>
        /// <param name="metaDataId">The meta data identifier.</param>
        /// <returns>Task&lt;MetaData&gt;.</returns>
        public async Task<MetaData> FetchMetaDataAsync(string metaDataId)
        {
            Check.ThrowIfNullOrEmpty(metaDataId, nameof(metaDataId));

            var fetched = await Heidelpay.RestClient.HttpGetAsync<Dictionary<string, string>>(BuildUri<MetaData>(metaDataId));

            return new MetaData { Id = metaDataId, MetadataMap = fetched };
        }

        /// <summary>
        /// Shipment as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>Task&lt;Shipment&gt;.</returns>
        public async Task<Shipment> ShipmentAsync(string paymentId, string invoiceId = null)
        {
            var shipment = new Shipment { InvoiceId = invoiceId };

            var paymentUri = BuildUri(Registry.ResolvePaymentUrl<Shipment>(paymentId), null);
            var result = await ApiPostAsync(shipment, paymentUri, false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// Charge as an asynchronous operation.
        /// </summary>
        /// <param name="charge">The charge.</param>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        internal async Task<Charge> ChargeAsync(Charge charge, string paymentId)
        {
            Check.ThrowIfNull(charge, nameof(charge));
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            var result = await ApiPostAsync(charge, BuildUri(Registry.ResolvePaymentUrl<Charge>(paymentId), null), getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// Cancel as an asynchronous operation.
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        internal async Task<Cancel> CancelAsync(Cancel cancel, string paymentId)
        {
            Check.ThrowIfNull(cancel, nameof(cancel));
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            var result = await ApiPostAsync(cancel, BuildUri(Registry.ResolvePaymentUrl<Cancel>(paymentId), null), getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.PaymentId);

            return result;
        }

        /// <summary>
        /// Cancel charge as an asynchronous operation.
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> CancelChargeAsync(Cancel cancel, string chargeId, string paymentId)
        { 
            Check.ThrowIfNull(cancel, nameof(cancel));
            Check.ThrowIfNullOrEmpty(chargeId, nameof(chargeId));
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            var result = await ApiPostAsync(cancel, 
                BuildUri(Registry.ResolveRefundUrl(paymentId, chargeId), null), 
                getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// Fetch authorization as an asynchronous operation.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        private async Task<Authorization> FetchAuthorizationAsync(Payment payment)
        {
            var authTransactions = GetAuthorization(payment.Transactions);
            var result = new List<Authorization>();
            foreach (var authTransaction in authTransactions)
            {
                var auth = await ApiGetAsync<Authorization>(authTransaction.Url);
                auth.Payment = payment;
                auth.ResourceUrl = authTransaction.Url;
                auth.TransactionType = authTransaction.Type;
                auth.CancelList = GetCancelsForAuthorization(payment.CancelList);
                auth.Resources.BasketId = payment.Resources.BasketId;
                result.Add(auth);
            }
            return result.SingleOrDefault();
        }

        /// <summary>
        /// Fetch cancel list as an asynchronous operation.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <returns>Task&lt;IEnumerable&lt;Cancel&gt;&gt;.</returns>
        private async Task<IEnumerable<Cancel>> FetchCancelListAsync(Payment payment)
        {
            var cancelTransactions = GetCancels(payment.Transactions);

            var result = new List<Cancel>();
            foreach (var cancelTransaction in cancelTransactions)
            {
                var cancel = await ApiGetAsync<Cancel>(cancelTransaction.Url);
                cancel.Payment = payment;
                cancel.ResourceUrl = cancelTransaction.Url;
                cancel.TransactionType = cancelTransaction.Type;
                result.Add(cancel);
            }
            return result;
        }

        private async Task<IEnumerable<Payout>> FetchPayoutListAsync(Payment payment)
        {
            var payoutTransactions = GetPayouts(payment.Transactions);

            var result = new List<Payout>();
            foreach (var payoutTransaction in payoutTransactions)
            {
                var payout = await ApiGetAsync<Payout>(payoutTransaction.Url);
                payout.Payment = payment;
                payout.ResourceUrl = payoutTransaction.Url;
                payout.TransactionType = payoutTransaction.Type;
                result.Add(payout);
            }
            return result;
        }

        /// <summary>
        /// Fetch charge list as an asynchronous operation.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <returns>Task&lt;IEnumerable&lt;Charge&gt;&gt;.</returns>
        private async Task<IEnumerable<Charge>> FetchChargeListAsync(Payment payment)
        {
            var chargeTransactions = GetCharges(payment.Transactions);
            var result = new List<Charge>();
            foreach (var chargeTransaction in chargeTransactions)
            {
                var charge = await ApiGetAsync<Charge>(chargeTransaction.Url);
                charge.Payment = payment;
                charge.ResourceUrl = chargeTransaction.Url;
                charge.CancelList = GetCancelsForCharge(payment.CancelList);
                charge.TransactionType = chargeTransaction.Type;
                charge.Resources.BasketId = payment.Resources.BasketId;
                result.Add(charge);
            }
            return result;
        }

        /// <summary>
        /// Gets the cancels for authorization.
        /// </summary>
        /// <param name="cancelList">The cancel list.</param>
        /// <returns>IEnumerable&lt;Cancel&gt;.</returns>
        private IEnumerable<Cancel> GetCancelsForAuthorization(IEnumerable<Cancel> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CANCEL_AUTHORIZE.Equals(x.TransactionType, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Payouts as an asynchronous operation.
        /// </summary>
        /// <param name="payout">The payout.</param>
        /// <returns></returns>
        public async Task<Payout> PayoutAsync(Payout payout)
        {
            Check.ThrowIfNull(payout, nameof(payout));

            var result = await ApiPostAsync(payout, getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// Recurrings as an asynchronous operation.
        /// </summary>
        /// <param name="recurring">The recurring.</param>
        /// <returns></returns>
        public async Task<Recurring> RecurringAsync(Recurring recurring)
        {
            var result = await ApiPostAsync(
                recurring, 
                uri: BuildUri(Registry.ResolveRecurringUrl(recurring.TypeId), null), 
                getAfterPost: false);

            return result;
        }

        /// <summary>
        /// Hires the purchase plan as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="effectiveInterestRate">The effective interest rate.</param>
        /// <param name="orderDate">The order date.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<HirePurchaseRatePlan>> HirePurchasePlanAsync(decimal amount, string currency, decimal effectiveInterestRate, DateTime orderDate)
        {
            var uri = BuildHirePurchaseUri(amount, currency, effectiveInterestRate, orderDate);

            var result = await ApiGetAsync<HirePurchaseRatePlanList>(uri: uri);

            return result.Entity;
        }


        /// <summary>
        /// Gets the cancels for charge.
        /// </summary>
        /// <param name="cancelList">The cancel list.</param>
        /// <returns>IEnumerable&lt;Cancel&gt;.</returns>
        private IEnumerable<Cancel> GetCancelsForCharge(IEnumerable<Cancel> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CANCEL_CHARGE.Equals(x.TransactionType, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Gets the cancels.
        /// </summary>
        /// <param name="cancelList">The cancel list.</param>
        /// <returns>IEnumerable&lt;Transaction&gt;.</returns>
        private IEnumerable<Transaction> GetCancels(IEnumerable<Transaction> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CANCEL_CHARGE.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase) 
                || TRANSACTION_TYPE_CANCEL_AUTHORIZE.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private IEnumerable<Transaction> GetPayouts(IEnumerable<Transaction> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_PAYOUT.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Gets the charges.
        /// </summary>
        /// <param name="cancelList">The cancel list.</param>
        /// <returns>IEnumerable&lt;Transaction&gt;.</returns>
        private IEnumerable<Transaction> GetCharges(IEnumerable<Transaction> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CHARGE.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Gets the authorization.
        /// </summary>
        /// <param name="cancelList">The cancel list.</param>
        /// <returns>IEnumerable&lt;Transaction&gt;.</returns>
        private IEnumerable<Transaction> GetAuthorization(IEnumerable<Transaction> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_AUTHORIZATION.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Posts the process payment.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <returns>Task&lt;Payment&gt;.</returns>
        private async Task<Payment> PostProcessPayment(Payment payment)
        {
            payment.CancelList = await FetchCancelListAsync(payment);
            payment.ChargesList = await FetchChargeListAsync(payment);
            payment.PayoutList = await FetchPayoutListAsync(payment);
            payment.Authorization = await FetchAuthorizationAsync(payment);
            return payment;
        }
    }
}
