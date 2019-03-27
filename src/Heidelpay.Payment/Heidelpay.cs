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

        public async Task<Charge> ChargeAuthorizationAsync(string paymentId, decimal? amount = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Cancel> CancelAuthorizationAsync(string paymentId, decimal? amount = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, string typeId, Uri returnUrl = null, string customerId = null)
        {
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
            return await ChargeAsync(amount, currency, paymentType, null);
        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, Customer customer = null)
        {
            return await ChargeAsync(amount, currency, paymentType, returnUrl, customer?.Id);
        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, string customerId)
        {
            var typeId = await EnsureTypeIdCreated(paymentType);
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
        
        public async Task<Charge> ChargeAsync(Charge charge)
        {
            Check.NotNull(charge, nameof(charge));
            
            return await PaymentService.ChargeAsync(charge);
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, string typeId, Uri returnUrl = null, string customerId = null)
        {
            return await AuthorizeAsync(new Authorization
            {
                Amount = amount,
                Currency = currency,
                Type = typeId,
                ReturnUrl = returnUrl,
                Resources = new Resources
                {
                    CustomerId = customerId,
                }
            });
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType)
        {
            var typeId = await EnsureTypeIdCreated(paymentType);

            return await AuthorizeAsync(amount, currency, typeId: typeId);
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, string customerId)
        {
            var typeId = await EnsureTypeIdCreated(paymentType);

            return await AuthorizeAsync(amount, currency, typeId: typeId, returnUrl: returnUrl, customerId: customerId);
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, Customer customer = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Authorization> AuthorizeAsync(Authorization authorization)
        {
            return await PaymentService.AuthorizeAsync(authorization);
        }

        public async Task<Cancel> CancelChargeAsync(string paymentId, string chargeId, decimal? amount = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> FetchCustomerAsync(string customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentTypeBase> FetchPaymentTypeAsync(string typeId)
        {
            return await PaymentService.FetchPaymentTypeAsync<PaymentTypeBase>(typeId);
        }
        
        public async Task<MetaData> FetchMetaDataAsync(string metaDataId)
        {
            throw new NotImplementedException();
        }

        public async Task<Basket> FetchBasketAsync(string basketId)
        {
            throw new NotImplementedException();
        }

        public async Task<Authorization> FetchAuthorizationAsync(string paymentId)
        {
            return await PaymentService.FetchAuthorizationAsync(paymentId);
        }

        public async Task<TPaymentBase> CreatePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentType)
             where TPaymentBase : PaymentTypeBase
        {
            return await PaymentService.CreatePaymentTypeBaseAsync(paymentType);
        }

        private IRestClient BuildRestClient(IHttpClientFactory httpClientFactory, IOptions<HeidelpayApiOptions> options)
        {
            return new RestClient(httpClientFactory, options, new NullLogger<RestClient>());
        }

        private async Task<string> EnsureTypeIdCreated<TPaymentType>(TPaymentType paymentType)
            where TPaymentType : IPaymentType
        {
            if (paymentType == null)
                return null;

            TPaymentType result = paymentType;
            if (string.IsNullOrEmpty(paymentType?.Id))
            {
                result = await PaymentService.EnsurePaymentTypeAsync(paymentType);
            }

            return result.Id;
        }
    }
}
