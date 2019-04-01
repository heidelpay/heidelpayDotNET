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

/*-
 * #%L
 * Heidelpay .NET SDK
 * %%
 * Copyright (C) 2019 Heidelpay GmbH
 * %%
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * #L%
 */

namespace Heidelpay.Payment
{
    public sealed class Heidelpay
    {
        internal IRestClient RestClient { get; }
        internal PaymentService PaymentService { get; } 
       
        public Heidelpay(HeidelpayApiOptions options, HttpClient httpClient)
            : this(Microsoft.Extensions.Options.Options.Create(options), httpClient)
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(httpClient, nameof(httpClient));
        }

        public Heidelpay(IOptions<HeidelpayApiOptions> options, HttpClient httpClient)
            : this()
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(httpClient, nameof(httpClient));

            RestClient = BuildRestClient(new WrappedHttpClientFactory(httpClient), options);
        }

        public Heidelpay(HeidelpayApiOptions options, IHttpClientFactory httpClientFactory)
            : this(Microsoft.Extensions.Options.Options.Create(options), httpClientFactory)
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(httpClientFactory, nameof(httpClientFactory));
        }

        public Heidelpay(IOptions<HeidelpayApiOptions> options, IHttpClientFactory httpClientFactory)
            : this()
        {
            Check.NotNull(options, nameof(options));
            Check.NotNull(httpClientFactory, nameof(httpClientFactory));

            RestClient = BuildRestClient(httpClientFactory, options);
        }

        public Heidelpay(IRestClient restClient)
            : this()
        {
            RestClient = restClient;
        }

        private Heidelpay()
        {
            PaymentService = new PaymentService(this);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            Check.NotNull(customer, nameof(customer));
            Check.ThrowIfTrue(!string.IsNullOrEmpty(customer.Id),
                "Customer has an id set. createCustomer can only be called without Customer.id. Please use updateCustomer or remove the id from Customer.");

            return await PaymentService.CreateCustomerAsync(customer);
        }

        public async Task<Customer> UpdateCustomerAsync(string id, Customer customer)
        {
            Check.NotNullOrEmpty(id, nameof(id));
            Check.NotNull(customer, nameof(customer));

            return await PaymentService.UpdateCustomerAsync(id, customer);
        }

        public async Task DeleteCustomerAsync(string id)
        {
            Check.NotNullOrEmpty(id, nameof(id));

            await PaymentService.DeleteCustomerAsync(id);
        }

        public async Task<Charge> ChargeAuthorizationAsync(string paymentId, decimal? amount = null)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            throw new NotImplementedException();
        }

        public async Task<Cancel> CancelAuthorizationAsync(string paymentId, decimal? amount = null)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            throw new NotImplementedException();
        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, string typeId, Uri returnUrl = null, string customerId = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNullOrEmpty(typeId, nameof(typeId));

            return await ChargeAsync(new Charge
            {
                Amount = amount,
                Currency = currency, ReturnUrl = returnUrl,
                Resources = new Resources
                {
                    TypeId = typeId,
                    CustomerId = customerId
                }
            });
        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, IPaymentType paymentType)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));

            return await ChargeAsync(amount, currency, paymentType, returnUrl: null, customerId: null);
        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, Customer customer = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));
            Check.NotNull(returnUrl, nameof(returnUrl));

            var customerId = await EnsureRestResourceCreatedAsync(customer);

            return await ChargeAsync(amount, currency, paymentType,  returnUrl, customerId);
        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, string customerId)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));
            Check.NotNull(returnUrl, nameof(returnUrl));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);

            return await ChargeAsync(new Charge
            {
                Amount = amount,
                Currency = currency,
                ReturnUrl = returnUrl,
                Resources = new Resources
                {
                    TypeId = typeId,
                    CustomerId = customerId
                }
            });
        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, Customer customer, Basket basket, string invoiceId = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));
            Check.NotNull(returnUrl, nameof(returnUrl));
            Check.NotNull(basket, nameof(basket));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);
            var customerId = await EnsureRestResourceCreatedAsync(customer);
            var basketId = await EnsureRestResourceCreatedAsync(basket);

            return await ChargeAsync(new Charge
            {
                Amount = amount,
                Currency = currency,
                ReturnUrl = returnUrl,
                InvoiceId = invoiceId,
                Resources = new Resources
                {
                    TypeId = typeId,
                    CustomerId = customerId,
                    BasketId = basketId,
                }
            });
        }

        public async Task<Charge> ChargeAsync(Charge charge)
        {
            Check.NotNull(charge, nameof(charge));
            
            return await PaymentService.ChargeAsync(charge);
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, string typeId, Uri returnUrl = null, string customerId = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNullOrEmpty(typeId, nameof(typeId));

            return await AuthorizeAsync(new Authorization
            {
                Amount = amount,
                Currency = currency,
                ReturnUrl = returnUrl,
                Resources = new Resources
                {
                    TypeId = typeId,
                    CustomerId = customerId,
                }
            });
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);

            return await AuthorizeAsync(amount, currency, typeId: typeId);
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, string customerId)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));
            Check.NotNullOrEmpty(customerId, nameof(customerId));
            Check.NotNull(returnUrl, nameof(returnUrl));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);

            return await AuthorizeAsync(amount, currency, typeId: typeId, returnUrl: returnUrl, customerId: customerId);
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, Customer customer = null)
        {
            Check.NotNullOrEmpty(currency, nameof(currency));
            Check.NotNull(paymentType, nameof(paymentType));
            Check.NotNull(returnUrl, nameof(returnUrl));

            var typeId = await EnsureRestResourceCreatedAsync(paymentType);
            var customerId = await EnsureRestResourceCreatedAsync(customer);

            return await AuthorizeAsync(amount, currency, typeId: typeId, returnUrl: returnUrl, customerId: customerId);
        }

        public async Task<Authorization> AuthorizeAsync(Authorization authorization)
        {
            return await PaymentService.AuthorizeAsync(authorization);
        }

        public async Task<Cancel> CancelChargeAsync(string paymentId, string chargeId, decimal? amount = null)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            Check.NotNullOrEmpty(chargeId, nameof(chargeId));

            throw new NotImplementedException();
        }

        public async Task<Charge> FetchChargeAsync(string paymentId, string chargeId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            Check.NotNullOrEmpty(chargeId, nameof(chargeId));

            return await PaymentService.FetchChargeAsync(paymentId, chargeId);
        }

        public async Task<Customer> FetchCustomerAsync(string customerId)
        {
            Check.NotNullOrEmpty(customerId, nameof(customerId));

            throw new NotImplementedException();
        }

        public async Task<TPaymentType> FetchPaymentTypeAsync<TPaymentType>(string typeId)
            where TPaymentType : PaymentTypeBase
        {
            Check.NotNullOrEmpty(typeId, nameof(typeId));

            return await PaymentService.FetchPaymentTypeAsync<TPaymentType>(typeId);
        }
        
        public async Task<MetaData> FetchMetaDataAsync(string metaDataId)
        {
            Check.NotNullOrEmpty(metaDataId, nameof(metaDataId));

            throw new NotImplementedException();
        }

        public async Task<Basket> FetchBasketAsync(string basketId)
        {
            Check.NotNullOrEmpty(basketId, nameof(basketId));

            throw new NotImplementedException();
        }

        public async Task<Authorization> FetchAuthorizationAsync(string paymentId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.FetchAuthorizationAsync(paymentId);
        }

        public async Task<TPaymentBase> CreatePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentType)
             where TPaymentBase : PaymentTypeBase
        {
            Check.NotNull(paymentType, nameof(paymentType));

            return await PaymentService.CreatePaymentTypeAsync(paymentType);
        }

        public async Task<Shipment> ShipmentAsync(string paymentId, string invoiceId = null)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            return await PaymentService.ShipmentAsync(paymentId, invoiceId);
        }

        private IRestClient BuildRestClient(IHttpClientFactory httpClientFactory, IOptions<HeidelpayApiOptions> options)
        {
            return new RestClient(httpClientFactory, options, new NullLogger<RestClient>());
        }

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
