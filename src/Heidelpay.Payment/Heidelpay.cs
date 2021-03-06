﻿// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="Heidelpay.cs" company="Heidelpay">
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

using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.Options;
using Heidelpay.Payment.PaymentTypes;
using Heidelpay.Payment.Service;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class Heidelpay. This class cannot be inherited.
    /// The Heidelpay instance is a facade to the Heidelpay REST Api. The facade is initialized with an instance of HeidelpayApiOptions.
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.Interfaces.IHeidelpay" />
    public sealed class HeidelpayClient : IHeidelpay
    {
        /// <summary>
        /// Gets the rest client.
        /// </summary>
        /// <value>The rest client.</value>
        internal IRestClient RestClient { get; }

        /// <summary>
        /// Gets the payment service.
        /// </summary>
        /// <value>The payment service.</value>
        internal PaymentApiService PaymentService { get; }
        internal PaypageApiService PaypageService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeidelpayClient"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public HeidelpayClient(string apiKey)
            : this(Microsoft.Extensions.Options.Options.Create(HeidelpayApiOptions.BuildDefault(apiKey)), new HttpClient())
        {
            Check.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeidelpayClient"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public HeidelpayClient(string apiKey, HttpClient httpClient)
            : this(Microsoft.Extensions.Options.Options.Create(HeidelpayApiOptions.BuildDefault(apiKey)), httpClient)
        {
            Check.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));
            Check.ThrowIfNull(httpClient, nameof(httpClient));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeidelpayClient"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        public HeidelpayClient(string apiKey, IHttpClientFactory httpClientFactory)
            : this(Microsoft.Extensions.Options.Options.Create(HeidelpayApiOptions.BuildDefault(apiKey)), httpClientFactory)
        {
            Check.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));
            Check.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heidelpay"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public HeidelpayClient(HeidelpayApiOptions options, HttpClient httpClient)
            : this(Microsoft.Extensions.Options.Options.Create(options), httpClient)
        {
            Check.ThrowIfNull(options, nameof(options));
            Check.ThrowIfNull(httpClient, nameof(httpClient));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heidelpay"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public HeidelpayClient(IOptions<HeidelpayApiOptions> options, HttpClient httpClient)
            : this()
        {
            Check.ThrowIfNull(options, nameof(options));
            Check.ThrowIfNull(httpClient, nameof(httpClient));

            RestClient = BuildRestClient(new WrappedHttpClientFactory(httpClient), options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heidelpay"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        public HeidelpayClient(HeidelpayApiOptions options, IHttpClientFactory httpClientFactory)
            : this(Microsoft.Extensions.Options.Options.Create(options), httpClientFactory)
        {
            Check.ThrowIfNull(options, nameof(options));
            Check.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heidelpay"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        public HeidelpayClient(IOptions<HeidelpayApiOptions> options, IHttpClientFactory httpClientFactory)
            : this()
        {
            Check.ThrowIfNull(options, nameof(options));
            Check.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));

            RestClient = BuildRestClient(httpClientFactory, options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heidelpay"/> class.
        /// </summary>
        /// <param name="restClient">The rest client.</param>
        public HeidelpayClient(IRestClient restClient)
            : this()
        {
            RestClient = restClient;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Heidelpay"/> class from being created.
        /// </summary>
        private HeidelpayClient()
        {
            PaymentService = new PaymentApiService(this);
            PaypageService = new PaypageApiService(this);
        }

        /// <summary>
        /// Create customer as an asynchronous operation.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            Check.ThrowIfNull(customer, nameof(customer));
            Check.ThrowIfTrue(!string.IsNullOrEmpty(customer.Id),
                "Customer has an id set. CreateCustomerAsync can only be called without Customer.id. Please use UpdateCustomerAsync or remove the id from Customer.");

            return await PaymentService.CreateCustomerAsync(customer);
        }

        /// <summary>
        /// Create basket as an asynchronous operation.
        /// </summary>
        /// <param name="basket">The basket.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> CreateBasketAsync(Basket basket)
        {
            Check.ThrowIfNull(basket, nameof(basket));
            Check.ThrowIfTrue(!string.IsNullOrEmpty(basket.Id),
                "Basket has an id set. CreateBasketAsync can only be called without Basket.id. Please use UpdateBasketAsync or remove the id from Basket.");

            return await PaymentService.CreateBasketAsync(basket);
        }

        /// <summary>
        /// Create metadata as an asynchronous operation.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>Task&lt;MetaData&gt;.</returns>
        public async Task<MetaData> CreateMetadataAsync(MetaData metadata)
        {
            Check.ThrowIfNull(metadata, nameof(metadata));
            Check.ThrowIfTrue(!string.IsNullOrEmpty(metadata.Id),
                "MetaData has an id set. CreateMetadataAsync can only be called without MetaData.id. Please use UpdateMetaDataAsync or remove the id from MetaData.");

            return await PaymentService.CreateMetaDataAsync(metadata);
        }

        /// <summary>
        /// Update customer as an asynchronous operation.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            Check.ThrowIfNull(customer, nameof(customer));
            Check.ThrowIfTrue(string.IsNullOrEmpty(customer.Id),
               "Customer has no id set. UpdateCustomerAsync can only be called with Customer.id.");

            return await PaymentService.UpdateCustomerAsync(customer);
        }

        /// <summary>
        /// Update basket as an asynchronous operation.
        /// </summary>
        /// <param name="basket">The basket.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> UpdateBasketAsync(Basket basket)
        {
            Check.ThrowIfNull(basket, nameof(basket));
            Check.ThrowIfTrue(string.IsNullOrEmpty(basket.Id),
               "Basket has no id set. UpdateBasketAsync can only be called with Basket.id.");

            return await PaymentService.UpdateBasketAsync(basket);
        }

        /// <summary>
        /// Delete customer as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        public async Task DeleteCustomerAsync(string id)
        {
            Check.ThrowIfNullOrEmpty(id, nameof(id));

            await PaymentService.DeleteCustomerAsync(id);
        }

        /// <summary>
        /// Charge authorization as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAuthorizationAsync(string paymentId, decimal? amount = null)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.ChargeAsync(new Charge { Amount = amount }, paymentId);
        }

        /// <summary>
        /// Cancel authorization as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> CancelAuthorizationAsync(string paymentId, decimal? amount = null)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.CancelAsync(new Cancel { Amount = amount }, paymentId);
        }

        /// <summary>
        /// Charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="chargeablePaymentTypeId">The chargeable payment type identifier.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="card3ds">if set to <c>true</c> [3ds] is set, overriding setting of the payment type.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(
            decimal amount,
            string currency,
            string chargeablePaymentTypeId,
            Uri returnUrl = null,
            string customerId = null,
            bool? card3ds = null)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));
            Check.ThrowIfNullOrEmpty(chargeablePaymentTypeId, nameof(chargeablePaymentTypeId));

            var paymentType = await PaymentService.FetchPaymentTypeAsync<PaymentTypeBase>(chargeablePaymentTypeId);
            Check.ThrowIfNull(paymentType as IChargeablePaymentType, "Only chargeable payment types are permitted for charging.");

            bool? threeDS = card3ds;
            if (card3ds == null && paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await ChargeAsync(new Charge
            {
                Amount = amount,
                Currency = currency,
                ReturnUrl = returnUrl,
                Card3ds = threeDS,
                TypeId = chargeablePaymentTypeId,
                CustomerId = customerId,
            });
        }

        /// <summary>
        /// Charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="card3ds">if set to <c>true</c> [3ds] is set, overriding setting of the payment type.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(
            decimal amount,
            string currency,
            IChargeablePaymentType paymentType,
            bool? card3ds = null)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));

            bool? threeDS = card3ds;
            if (card3ds == null && paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await ChargeAsync(amount, currency, paymentType, returnUrl: null, customerId: null, card3ds: threeDS);
        }

        /// <summary>
        /// Charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="card3ds">if set to <c>true</c> [3ds] is set, overriding setting of the payment type.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(
            decimal amount,
            string currency,
            IChargeablePaymentType paymentType,
            Uri returnUrl,
            Customer customer = null,
            bool? card3ds = null)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));
            Check.ThrowIfNull(paymentType, nameof(paymentType));
            Check.ThrowIfNull(returnUrl, nameof(returnUrl));

            var customerId = await EnsureRestResourceCreatedAsync(customer);

            bool? threeDS = card3ds;
            if (card3ds == null && paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await ChargeAsync(amount, currency, paymentType, returnUrl, customerId, threeDS);
        }

        /// <summary>
        /// Charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="card3ds">if set to <c>true</c> [3ds] is set, overriding setting of the payment type.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(
            decimal amount,
            string currency,
            IChargeablePaymentType paymentType,
            Uri returnUrl,
            string customerId,
            bool? card3ds = null)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));
            Check.ThrowIfNull(paymentType, nameof(paymentType));
            Check.ThrowIfNull(returnUrl, nameof(returnUrl));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);

            bool? threeDS = card3ds;
            if (card3ds == null && paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await ChargeAsync(new Charge
            {
                Amount = amount,
                Currency = currency,
                ReturnUrl = returnUrl,
                Card3ds = threeDS,
                TypeId = typeId,
                CustomerId = customerId
            });
        }

        /// <summary>
        /// Charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="basket">The basket.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <param name="card3ds">if set to <c>true</c> [3ds] is set, overriding setting of the payment type.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(
            decimal amount,
            string currency,
            IChargeablePaymentType paymentType,
            Uri returnUrl,
            Customer customer,
            Basket basket,
            string invoiceId = null,
            bool? card3ds = null)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));
            Check.ThrowIfNull(paymentType, nameof(paymentType));
            Check.ThrowIfNull(returnUrl, nameof(returnUrl));
            Check.ThrowIfNull(basket, nameof(basket));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);
            var customerId = await EnsureRestResourceCreatedAsync(customer);
            var basketId = await EnsureRestResourceCreatedAsync(basket);

            bool? threeDS = card3ds;
            if (card3ds == null && paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await ChargeAsync(new Charge
            {
                Amount = amount,
                Currency = currency,
                ReturnUrl = returnUrl,
                InvoiceId = invoiceId,
                Card3ds = threeDS,
                TypeId = typeId,
                CustomerId = customerId,
                BasketId = basketId,
            });
        }

        /// <summary>
        /// Charge as an asynchronous operation.
        /// </summary>
        /// <param name="charge">The charge.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(Charge charge)
        {
            Check.ThrowIfNull(charge, nameof(charge));

            return await PaymentService.ChargeAsync(charge);
        }

        /// <summary>
        /// Payout as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        public async Task<Payout> PayoutAsync(
            decimal amount,
            string currency,
            IChargeablePaymentType paymentType,
            Uri returnUrl)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);

            return await PayoutAsync(new Payout
            {
                Amount = amount,
                Currency = currency,
                TypeId = typeId,
                ReturnUrl = returnUrl,
            });
        }

        /// <summary>
        /// Payout as an asynchronous operation.
        /// </summary>
        /// <param name="payout">The payout.</param>
        /// <returns></returns>
        public async Task<Payout> PayoutAsync(Payout payout)
        {
            Check.ThrowIfNull(payout, nameof(payout));

            return await PaymentService.PayoutAsync(payout);
        }



        /// <summary>
        /// Recurrings the asynchronous.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="metadataId">The metadata identifier.</param>
        /// <returns></returns>
        public async Task<Recurring> RecurringAsync(
            IPaymentType paymentType,
            Uri returnUrl,
            string customerId = null,
            string metadataId = null) 
        {
            Check.ThrowIfNull(paymentType, nameof(paymentType));
            Check.ThrowIfNull(returnUrl, nameof(returnUrl));

            var recurring = new Recurring(this, paymentType)
            {
                CustomerId = customerId,
                ReturnUrl = returnUrl,
                MetadataId = metadataId,
            };

            return await PaymentService.RecurringAsync(recurring);
        }

        /// <summary>
        /// Authorize as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="authorizedPaymentTypeId">The authorized payment type identifier.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="card3ds">if set to <c>true</c> [3ds] is set, overriding setting of the payment type.</param>
        /// <returns>
        /// Task&lt;Authorization&gt;.
        /// </returns>
        public async Task<Authorization> AuthorizeAsync(
            decimal amount,
            string currency,
            string authorizedPaymentTypeId,
            Uri returnUrl = null,
            string customerId = null,
            bool? card3ds = null)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));
            Check.ThrowIfNullOrEmpty(authorizedPaymentTypeId, nameof(authorizedPaymentTypeId));

            var type = await PaymentService.FetchPaymentTypeAsync<PaymentTypeBase>(authorizedPaymentTypeId);
            Check.ThrowIfNull(type as IAuthorizedPaymentType, "Only authorizable payment types are permitted for authorization.");

            bool? threeDS = card3ds;
            if (card3ds == null && type is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await AuthorizeAsync(new Authorization
            {
                Amount = amount,
                Currency = currency,
                ReturnUrl = returnUrl,
                Card3ds = threeDS,
                TypeId = authorizedPaymentTypeId,
                CustomerId = customerId,
            });
        }

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
        public async Task<Authorization> AuthorizeAsync(
            decimal amount, 
            string currency, 
            string authorizedPaymentTypeId, 
            Uri returnUrl, 
            string customerId, 
            string basketId, 
            decimal? effectiveInterestRate = null)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));
            Check.ThrowIfNullOrEmpty(authorizedPaymentTypeId, nameof(authorizedPaymentTypeId));

            var type = await PaymentService.FetchPaymentTypeAsync<PaymentTypeBase>(authorizedPaymentTypeId);
            Check.ThrowIfNull(type as IAuthorizedPaymentType, "Only authorizable payment types are permitted for authorization.");

            return await AuthorizeAsync(new Authorization
            {
                Amount = amount,
                Currency = currency,
                ReturnUrl = returnUrl,
                TypeId = authorizedPaymentTypeId,
                CustomerId = customerId,
                BasketId = basketId,
                EffectiveInterestRate = effectiveInterestRate,
            });
        }

        /// <summary>
        /// Authorize as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IAuthorizedPaymentType paymentType)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));
            Check.ThrowIfNull(paymentType, nameof(paymentType));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);

            bool? threeDS = null;
            if(paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await AuthorizeAsync(amount, currency, authorizedPaymentTypeId: typeId, card3ds: threeDS);
        }

        /// <summary>
        /// Authorize as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(
            decimal amount, 
            string currency, 
            IAuthorizedPaymentType paymentType, 
            Uri returnUrl, 
            string customerId)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));
            Check.ThrowIfNull(paymentType, nameof(paymentType));
            Check.ThrowIfNullOrEmpty(customerId, nameof(customerId));
            Check.ThrowIfNull(returnUrl, nameof(returnUrl));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);

            bool? threeDS = null;
            if (paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await AuthorizeAsync(amount, currency, authorizedPaymentTypeId: typeId, returnUrl: returnUrl, customerId: customerId, card3ds: threeDS);
        }

        /// <summary>
        /// Authorize as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="card3ds">if set to <c>true</c> [3ds] is set, overriding setting of the payment type.</param>
        /// <returns>
        /// Task&lt;Authorization&gt;.
        /// </returns>
        public async Task<Authorization> AuthorizeAsync(
            decimal amount, 
            string currency, 
            IAuthorizedPaymentType paymentType,
            Uri returnUrl, 
            Customer customer = null,
            bool? card3ds = null)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));
            Check.ThrowIfNull(paymentType, nameof(paymentType));
            Check.ThrowIfNull(returnUrl, nameof(returnUrl));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);
            var customerId = await EnsureRestResourceCreatedAsync(customer);

            bool? threeDS = card3ds;
            if (card3ds == null && paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await AuthorizeAsync(
                amount, 
                currency, 
                authorizedPaymentTypeId: typeId, 
                returnUrl: returnUrl, 
                customerId: customerId, 
                card3ds: threeDS);
        }


        /// <summary>
        /// Authorizes as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="basket">The basket.</param>
        /// <param name="effectiveInterestRate">The effective interest rate.</param>
        /// <returns></returns>
        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IAuthorizedPaymentType paymentType, Uri returnUrl, Customer customer, Basket basket, decimal? effectiveInterestRate = null)
        {
            Check.ThrowIfNullOrEmpty(currency, nameof(currency));
            Check.ThrowIfNull(paymentType, nameof(paymentType));
            Check.ThrowIfNull(returnUrl, nameof(returnUrl));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);
            var customerId = await EnsureRestResourceCreatedAsync(customer);
            var basketId = await EnsureRestResourceCreatedAsync(basket);

            return await AuthorizeAsync(
                amount,
                currency,
                authorizedPaymentTypeId: typeId,
                returnUrl: returnUrl,
                customerId: customerId,
                basketId: basketId,
                effectiveInterestRate: effectiveInterestRate);
        }

        /// <summary>
        /// Authorize as an asynchronous operation.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(Authorization authorization)
        {
            return await PaymentService.AuthorizeAsync(authorization);
        }

        /// <summary>
        /// Cancel charge as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> CancelChargeAsync(string paymentId, string chargeId, decimal? amount = null)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            Check.ThrowIfNullOrEmpty(chargeId, nameof(chargeId));

            return await PaymentService.CancelChargeAsync(new Cancel { Amount = amount }, chargeId, paymentId);
        }

        /// <summary>
        /// Cancels the charge as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <param name="amountGross">The amount gross.</param>
        /// <param name="amountNet">The amount net.</param>
        /// <param name="amountVat">The amount vat.</param>
        /// <returns></returns>
        public async Task<Cancel> CancelChargeAsync(string paymentId, string chargeId, decimal amountGross, decimal amountNet, decimal amountVat)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            Check.ThrowIfNullOrEmpty(chargeId, nameof(chargeId));

            return await PaymentService.CancelChargeAsync(new Cancel
            {
                AmountGross = amountGross,
                AmountNet = amountNet,
                AmountVat = amountVat,
            }, chargeId, paymentId);
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

            return await PaymentService.FetchChargeAsync(paymentId, chargeId);
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

            return await PaymentService.FetchPayoutAsync(paymentId, payoutId);
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

            return await PaymentService.FetchCancelAsync(paymentId, cancelId);
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
            Check.ThrowIfNullOrEmpty(chargeId, nameof(chargeId));
            Check.ThrowIfNullOrEmpty(cancelId, nameof(cancelId));

            return await PaymentService.FetchCancelAsync(paymentId, chargeId, cancelId);
        }

        /// <summary>
        /// Fetch customer as an asynchronous operation.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> FetchCustomerAsync(string customerId)
        {
            Check.ThrowIfNullOrEmpty(customerId, nameof(customerId));

            return await PaymentService.FetchCustomerAsync(customerId);
        }

        /// <summary>
        /// Fetch payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentType">The type of the t payment type.</typeparam>
        /// <param name="typeId">The type identifier.</param>
        /// <returns>Task&lt;TPaymentType&gt;.</returns>
        public async Task<TPaymentType> FetchPaymentTypeAsync<TPaymentType>(string typeId)
            where TPaymentType : PaymentTypeBase
        {
            Check.ThrowIfNullOrEmpty(typeId, nameof(typeId));

            return await PaymentService.FetchPaymentTypeAsync<TPaymentType>(typeId);
        }

        /// <summary>
        /// Fetch meta data as an asynchronous operation.
        /// </summary>
        /// <param name="metaDataId">The meta data identifier.</param>
        /// <returns>Task&lt;MetaData&gt;.</returns>
        public async Task<MetaData> FetchMetaDataAsync(string metaDataId)
        {
            Check.ThrowIfNullOrEmpty(metaDataId, nameof(metaDataId));

            return await PaymentService.FetchMetaDataAsync(metaDataId);
        }

        /// <summary>
        /// fetch basket as an asynchronous operation.
        /// </summary>
        /// <param name="basketId">The basket identifier.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> FetchBasketAsync(string basketId)
        {
            Check.ThrowIfNullOrEmpty(basketId, nameof(basketId));

            return await PaymentService.FetchBasketAsync(basketId);
        }

        /// <summary>
        /// Fetch authorization as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> FetchAuthorizationAsync(string paymentId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.FetchAuthorizationAsync(paymentId);
        }

        /// <summary>
        /// Fetch payment as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Payment&gt;.</returns>
        public async Task<Payment> FetchPaymentAsync(string paymentId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.FetchPaymentAsync(paymentId);
        }

        /// <summary>
        /// Create payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentBase">The type of the t payment base.</typeparam>
        /// <param name="config">The configuration.</param>
        /// <returns>Task&lt;TPaymentBase&gt;.</returns>
        public async Task<TPaymentBase> CreatePaymentTypeAsync<TPaymentBase>(Action<TPaymentBase> config = null)
             where TPaymentBase : PaymentTypeBase
        {
            var instance = (TPaymentBase)Activator.CreateInstance(typeof(TPaymentBase), nonPublic: true);

            config?.Invoke(instance);

            return await PaymentService.CreatePaymentTypeAsync(instance);
        }

        /// <summary>
        /// Creates the payment type asynchronous.
        /// </summary>
        /// <typeparam name="TPaymentBase">The type of the payment base.</typeparam>
        /// <param name="paymentTypeInstance">The payment type instance.</param>
        /// <returns></returns>
        public async Task<TPaymentBase> CreatePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentTypeInstance)
            where TPaymentBase : PaymentTypeBase
        {
            return await PaymentService.CreatePaymentTypeAsync(paymentTypeInstance);
        }

        /// <summary>
        /// Updates the payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentBase">The type of the payment base.</typeparam>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        public async Task<TPaymentBase> UpdatePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentType)
             where TPaymentBase : PaymentTypeBase
        {
            Check.ThrowIfNull(paymentType, nameof(paymentType));

            if (string.IsNullOrEmpty(paymentType.Id))
                return await PaymentService.CreatePaymentTypeAsync(paymentType);

            return await PaymentService.UpdatePaymentTypeAsync(paymentType);
        }

        /// <summary>
        /// Shipment as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>Task&lt;Shipment&gt;.</returns>
        public async Task<Shipment> ShipmentAsync(string paymentId, string invoiceId = null)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.ShipmentAsync(paymentId, invoiceId);
        }

        /// <summary>
        /// Paypages as an asynchronous operation.
        /// </summary>
        /// <param name="paypage">The paypage.</param>
        /// <returns></returns>
        public async Task<Paypage> PaypageAsync(Paypage paypage)
        {
            Check.ThrowIfNull(paypage, nameof(paypage));

            return await PaypageService.InitializeAsync(paypage);
        }

        /// <summary>
        /// Hires the purchase rates asynchronous.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="effectiveInterestRate">The effective interest rate.</param>
        /// <param name="orderDate">The order date.</param>
        /// <returns></returns>
        public async Task<IEnumerable<HirePurchaseRatePlan>> HirePurchaseRatesAsync(decimal amount, string currency, decimal effectiveInterestRate, DateTime orderDate)
        {
            return await PaymentService.HirePurchasePlanAsync(amount, currency, effectiveInterestRate, orderDate);
        }

        /// <summary>
        /// Builds the rest client.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="options">The options.</param>
        /// <returns>IRestClient.</returns>
        private IRestClient BuildRestClient(IHttpClientFactory httpClientFactory, IOptions<HeidelpayApiOptions> options)
        {
            Check.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));
            Check.ThrowIfNull(options?.Value, nameof(options));

            return new RestClient(httpClientFactory.CreateClient(), options, new NullLogger<RestClient>());
        }

        /// <summary>
        /// ensure rest resource created as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="restResource">The rest resource.</param>
        /// <param name="resourceUrl">The resource URL.</param>
        /// <returns>
        /// Task&lt;System.String&gt;.
        /// </returns>
        private async Task<string> EnsureRestResourceCreatedAsync<T>(T restResource, string resourceUrl = null)
            where T : class, IRestResource
        {
            if (restResource == null)
                return null;

            string resultId = restResource?.Id;
            if (string.IsNullOrEmpty(resultId))
            {
                resultId = await PaymentService.EnsureRestResourceIdAsync(restResource, resourceUrl);
            }

            return resultId;
        }


    }
}
