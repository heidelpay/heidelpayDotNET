using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.PaymentTypes;
using System;
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
        public string PrivateKey { get; }
        public IRestClient RestClient { get; }

        public Heidelpay(string privateKey)
        {
            PrivateKey = privateKey;
        }

        public Heidelpay(string privateKey, IRestClient restClient)
            : this(privateKey)
        {
            RestClient = restClient;
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

        public Task<Charge> ChargeAsync(decimal amount, string currency, PaymentType paymentType)
        {
            throw new NotImplementedException();
        }

        public Task<Charge> ChargeAsync(decimal amount, string currency, PaymentType paymentType, Uri returnUrl, string customerId)
        {
            throw new NotImplementedException();
        }

        internal Task<Authorization> AuthorizeAsync(decimal amount, string currency, string typeId, Uri returnUrl = null, string customerId = null)
        {
            throw new NotImplementedException();
        }

        public Task<Authorization> AuthorizeAsync(decimal amount, string currency, PaymentType paymentType)
        {
            throw new NotImplementedException();
        }

        public Task<Authorization> AuthorizeAsync(decimal amount, string currency, PaymentType paymentType, Uri returnUrl, string customerId)
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

        public async Task<PaymentType> FetchPaymentTypeAsync(string paymentTypeId)
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
