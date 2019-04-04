﻿// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="Heidelpay.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.Options;
using Heidelpay.Payment.PaymentTypes;
using Heidelpay.Payment.Service;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class Heidelpay. This class cannot be inherited.
    /// 
    /// The Heidelpay instance is a facade to the Heidelpay REST Api. The facade is initialized with an instance of HeidelpayApiOptions.
    /// </summary>
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
        internal PaymentService PaymentService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heidelpay"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public HeidelpayClient(HeidelpayApiOptions options, HttpClient httpClient)
            : this(Microsoft.Extensions.Options.Options.Create(options), httpClient)
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(httpClient, nameof(httpClient));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heidelpay"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public HeidelpayClient(IOptions<HeidelpayApiOptions> options, HttpClient httpClient)
            : this()
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(httpClient, nameof(httpClient));

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
            Check.NotNull(options, nameof(options));
            Check.NotNull(httpClientFactory, nameof(httpClientFactory));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heidelpay"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        public HeidelpayClient(IOptions<HeidelpayApiOptions> options, IHttpClientFactory httpClientFactory)
            : this()
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(httpClientFactory, nameof(httpClientFactory));

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
            PaymentService = new PaymentService(this);
        }

        /// <summary>
        /// create customer as an asynchronous operation.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            Check.NotNull(customer, nameof(customer));
            Check.ThrowIfTrue(!string.IsNullOrEmpty(customer.Id),
                "Customer has an id set. CreateCustomerAsync can only be called without Customer.id. Please use UpdateCustomerAsync or remove the id from Customer.");

            return await PaymentService.CreateCustomerAsync(customer);
        }

        /// <summary>
        /// create basket as an asynchronous operation.
        /// </summary>
        /// <param name="basket">The basket.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> CreateBasketAsync(Basket basket)
        {
            Check.NotNull(basket, nameof(basket));
            Check.ThrowIfTrue(!string.IsNullOrEmpty(basket.Id),
                "Basket has an id set. CreateBasketAsync can only be called without Basket.id. Please use UpdateBasketAsync or remove the id from Basket.");

            return await PaymentService.CreateBasketAsync(basket);
        }

        /// <summary>
        /// update customer as an asynchronous operation.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            Check.NotNull(customer, nameof(customer));
            Check.ThrowIfTrue(string.IsNullOrEmpty(customer.Id),
               "Customer has no id set. UpdateCustomerAsync can only be called with Customer.id.");

            return await PaymentService.UpdateCustomerAsync(customer.Id, customer);
        }

        /// <summary>
        /// update basket as an asynchronous operation.
        /// </summary>
        /// <param name="basket">The basket.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> UpdateBasketAsync(Basket basket)
        {
            Check.NotNull(basket, nameof(basket));
            Check.ThrowIfTrue(string.IsNullOrEmpty(basket.Id),
               "Basket has no id set. UpdateBasketAsync can only be called with Basket.id.");

            return await PaymentService.UpdateBasketAsync(basket.Id, basket);
        }

        /// <summary>
        /// delete customer as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        public async Task DeleteCustomerAsync(string id)
        {
            Check.NotNullOrEmpty(id, nameof(id));

            await PaymentService.DeleteCustomerAsync(id);
        }

        /// <summary>
        /// charge authorization as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAuthorizationAsync(string paymentId, decimal? amount = null)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.ChargeAsync(new Charge { Amount = amount }, paymentId);
        }

        /// <summary>
        /// cancel authorization as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> CancelAuthorizationAsync(string paymentId, decimal? amount = null)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.CancelAsync(new Cancel { Amount = amount }, paymentId);
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="chargeablePaymentTypeId">The chargeable payment type identifier.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="card3ds">if set to <c>true</c> [card3ds].</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(decimal amount, string currency, string chargeablePaymentTypeId, Uri returnUrl = null, string customerId = null, bool? card3ds = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNullOrEmpty(chargeablePaymentTypeId, nameof(chargeablePaymentTypeId));

            var paymentType = await PaymentService.FetchPaymentTypeAsync<PaymentTypeBase>(chargeablePaymentTypeId);
            Check.NotNull(paymentType as IChargeablePaymentType, "Only chargeable payment types are permitted for charging.");

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
                Resources = new Resources
                {
                    TypeId = chargeablePaymentTypeId,
                    CustomerId = customerId
                }
            });
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="card3ds">if set to <c>true</c> [card3ds].</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(decimal amount, string currency, IChargeablePaymentType paymentType, bool? card3ds = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));

            bool? threeDS = card3ds;
            if (card3ds == null && paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await ChargeAsync(amount, currency, paymentType, returnUrl: null, customerId: null, card3ds: threeDS);
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="card3ds">if set to <c>true</c> [card3ds].</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(decimal amount, string currency, IChargeablePaymentType paymentType, Uri returnUrl, Customer customer = null, bool? card3ds = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));
            Check.NotNull(returnUrl, nameof(returnUrl));

            var customerId = await EnsureRestResourceCreatedAsync(customer);

            bool? threeDS = card3ds;
            if (card3ds == null && paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await ChargeAsync(amount, currency, paymentType, returnUrl, customerId, threeDS);
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="card3ds">if set to <c>true</c> [card3ds].</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(decimal amount, string currency, IChargeablePaymentType paymentType, Uri returnUrl, string customerId, bool? card3ds = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));
            Check.NotNull(returnUrl, nameof(returnUrl));

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
                Resources = new Resources
                {
                    TypeId = typeId,
                    CustomerId = customerId
                }
            });
        }

        /// <summary>
        /// charge as an asynchronous operation.
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
        public async Task<Charge> ChargeAsync(decimal amount, string currency, IChargeablePaymentType paymentType, Uri returnUrl, Customer customer, Basket basket, string invoiceId = null, bool? card3ds = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));
            Check.NotNull(returnUrl, nameof(returnUrl));
            Check.NotNull(basket, nameof(basket));

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
                Resources = new Resources
                {
                    TypeId = typeId,
                    CustomerId = customerId,
                    BasketId = basketId,
                }
            });
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="charge">The charge.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(Charge charge)
        {
            Check.NotNull(charge, nameof(charge));

            return await PaymentService.ChargeAsync(charge);
        }

        /// <summary>
        /// authorize as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="authorizedPaymentTypeId">The authorized payment type identifier.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="card3ds"></param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, string authorizedPaymentTypeId, Uri returnUrl = null, string customerId = null, bool? card3ds = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNullOrEmpty(authorizedPaymentTypeId, nameof(authorizedPaymentTypeId));

            var type = await PaymentService.FetchPaymentTypeAsync<PaymentTypeBase>(authorizedPaymentTypeId);
            Check.NotNull(type as IAuthorizedPaymentType, "Only authorizable payment types are permitted for authorization.");

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
                Resources = new Resources
                {
                    TypeId = authorizedPaymentTypeId,
                    CustomerId = customerId,
                }
            });
        }

        /// <summary>
        /// authorize as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IAuthorizedPaymentType paymentType)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);

            bool? threeDS = null;
            if(paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await AuthorizeAsync(amount, currency, authorizedPaymentTypeId: typeId, card3ds: threeDS);
        }

        /// <summary>
        /// authorize as an asynchronous operation.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IAuthorizedPaymentType paymentType, Uri returnUrl, string customerId)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));
            Check.NotNullOrEmpty(customerId, nameof(customerId));
            Check.NotNull(returnUrl, nameof(returnUrl));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);

            bool? threeDS = null;
            if (paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await AuthorizeAsync(amount, currency, authorizedPaymentTypeId: typeId, returnUrl: returnUrl, customerId: customerId, card3ds: threeDS);
        }

        /// <summary>authorize as an asynchronous operation.</summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="paymentType">Type of the payment.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="card3ds">if set to <c>true</c> [card3ds].</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IAuthorizedPaymentType paymentType, Uri returnUrl, 
            Customer customer = null, bool? card3ds = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));
            Check.NotNull(returnUrl, nameof(returnUrl));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);
            var customerId = await EnsureRestResourceCreatedAsync(customer);

            bool? threeDS = card3ds;
            if (card3ds == null && paymentType is IProvide3DS threeDSprovider)
            {
                threeDS = threeDSprovider.ThreeDs;
            }

            return await AuthorizeAsync(amount, currency, authorizedPaymentTypeId: typeId, returnUrl: returnUrl, customerId: customerId, card3ds: threeDS);
        }

        /// <summary>
        /// authorize as an asynchronous operation.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(Authorization authorization)
        {
            return await PaymentService.AuthorizeAsync(authorization);
        }

        /// <summary>
        /// cancel charge as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> CancelChargeAsync(string paymentId, string chargeId, decimal? amount = null)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            Check.NotNullOrEmpty(chargeId, nameof(chargeId));

            return await PaymentService.CancelChargeAsync(new Cancel { Amount = amount }, chargeId, paymentId);
        }

        /// <summary>
        /// fetch charge as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> FetchChargeAsync(string paymentId, string chargeId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            Check.NotNullOrEmpty(chargeId, nameof(chargeId));

            return await PaymentService.FetchChargeAsync(paymentId, chargeId);
        }

        /// <summary>
        /// fetch cancel as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="cancelId">The cancel identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> FetchCancelAsync(string paymentId, string cancelId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            Check.NotNullOrEmpty(cancelId, nameof(cancelId));

            return await PaymentService.FetchCancelAsync(paymentId, cancelId);
        }

        /// <summary>
        /// fetch cancel as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <param name="cancelId">The cancel identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> FetchCancelAsync(string paymentId, string chargeId, string cancelId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            Check.NotNullOrEmpty(chargeId, nameof(chargeId));
            Check.NotNullOrEmpty(cancelId, nameof(cancelId));

            return await PaymentService.FetchCancelAsync(paymentId, chargeId, cancelId);
        }

        /// <summary>
        /// fetch customer as an asynchronous operation.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> FetchCustomerAsync(string customerId)
        {
            Check.NotNullOrEmpty(customerId, nameof(customerId));

            return await PaymentService.FetchCustomerAsync(customerId);
        }

        /// <summary>
        /// fetch payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentType">The type of the t payment type.</typeparam>
        /// <param name="typeId">The type identifier.</param>
        /// <returns>Task&lt;TPaymentType&gt;.</returns>
        public async Task<TPaymentType> FetchPaymentTypeAsync<TPaymentType>(string typeId)
            where TPaymentType : class, IPaymentType
        {
            Check.NotNullOrEmpty(typeId, nameof(typeId));

            return await PaymentService.FetchPaymentTypeAsync<TPaymentType>(typeId);
        }

        /// <summary>
        /// fetch meta data as an asynchronous operation.
        /// </summary>
        /// <param name="metaDataId">The meta data identifier.</param>
        /// <returns>Task&lt;MetaData&gt;.</returns>
        public async Task<MetaData> FetchMetaDataAsync(string metaDataId)
        {
            Check.NotNullOrEmpty(metaDataId, nameof(metaDataId));

            return await PaymentService.FetchMetaDataAsync(metaDataId);
        }

        /// <summary>
        /// fetch basket as an asynchronous operation.
        /// </summary>
        /// <param name="basketId">The basket identifier.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> FetchBasketAsync(string basketId)
        {
            Check.NotNullOrEmpty(basketId, nameof(basketId));

            return await PaymentService.FetchBasketAsync(basketId);
        }

        /// <summary>
        /// fetch authorization as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> FetchAuthorizationAsync(string paymentId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.FetchAuthorizationAsync(paymentId);
        }

        /// <summary>
        /// fetch payment as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Payment&gt;.</returns>
        public async Task<Payment> FetchPaymentAsync(string paymentId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.FetchPaymentAsync(paymentId);
        }

        /// <summary>
        /// create payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentBase">The type of the t payment base.</typeparam>
        /// <param name="config">The configuration.</param>
        /// <returns>Task&lt;TPaymentBase&gt;.</returns>
        public async Task<TPaymentBase> CreatePaymentTypeAsync<TPaymentBase>(Action<TPaymentBase> config = null)
             where TPaymentBase : class, IPaymentType
        {
            var instance = (TPaymentBase)Activator.CreateInstance(typeof(TPaymentBase), nonPublic: true);

            config?.Invoke(instance);

            return await PaymentService.CreatePaymentTypeAsync(instance);
        }

        /// <summary>
        /// shipment as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>Task&lt;Shipment&gt;.</returns>
        public async Task<Shipment> ShipmentAsync(string paymentId, string invoiceId = null)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.ShipmentAsync(paymentId, invoiceId);
        }

        /// <summary>
        /// Builds the rest client.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="options">The options.</param>
        /// <returns>IRestClient.</returns>
        private IRestClient BuildRestClient(IHttpClientFactory httpClientFactory, IOptions<HeidelpayApiOptions> options)
        {
            return new RestClient(httpClientFactory, options, new NullLogger<RestClient>());
        }

        /// <summary>
        /// ensure rest resource created as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="restResource">The rest resource.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> EnsureRestResourceCreatedAsync<T>(T restResource)
            where T : class, IRestResource
        {
            if (restResource == null)
                return null;

            string resultId = restResource?.Id;
            if (string.IsNullOrEmpty(resultId))
            {
                resultId = await PaymentService.EnsureRestResourceIdAsync(restResource);
            }

            return resultId;
        }
    }
}
