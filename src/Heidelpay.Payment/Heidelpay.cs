using System.Runtime.CompilerServices;
using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.Options;
using Heidelpay.Payment.PaymentTypes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Heidelpay.Payment.Service;
using Heidelpay.Payment.Extensions;

[assembly: InternalsVisibleTo("Heidelpay.Payment.Tests")]

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

        private IRestClient BuildRestClient(IHttpClientFactory httpClientFactory, IOptions<HeidelpayApiOptions> options)
        {
            return new RestClient(httpClientFactory, options, new NullLogger<RestClient>());
        }

        public async Task<Charge> ChargeAuthorizationAsync(string paymentId, decimal? amount = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Cancel> CancelAuthorizationAsync(string paymentId, decimal? amount = null)
        {
            throw new NotImplementedException();
        }

        internal Task<Charge> ChargeAsync(decimal amount, string currency, string typeId, Uri returnUrl = null, string customerId = null)
        {
            throw new NotImplementedException();
        }

        public Task<Charge> ChargeAsync(decimal amount, string currency, IPaymentType paymentType)
        {
            throw new NotImplementedException();
        }

        public Task<Charge> ChargeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, string customerId)
        {
            throw new NotImplementedException();
        }

        public Task<Charge> ChargeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, Customer customer = null)
        {
            throw new NotImplementedException();
        }

        internal Task<Authorization> AuthorizeAsync(decimal amount, string currency, string typeId, Uri returnUrl = null, string customerId = null)
        {
            throw new NotImplementedException();
        }

        public Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType)
        {
            throw new NotImplementedException();
        }

        public Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, string customerId)
        {
            throw new NotImplementedException();
        }

        public Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, Customer customer = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Cancel> CancelChargeAsync(string paymentId, string chargeId, decimal? amount = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> FetchCustomerAsync(string customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<IPaymentType> FetchPaymentTypeAsync(string paymentTypeId)
        {
            throw new NotImplementedException();
        }


        public async Task<MetaData> FetchMetaDataAsync(string metaDataId)
        {
            throw new NotImplementedException();
        }

        public async Task<Basket> FetchBasketAsync(string basketId)
        {
            throw new NotImplementedException();
        }
    }
}
