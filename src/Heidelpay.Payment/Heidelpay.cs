using System;
using System.Net.Http;
using System.Threading.Tasks;
using Heidelpay.Payment.PaymentTypes;
using Heidelpay.Payment.RestClient;

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

        public Heidelpay(string privateKey)
        {
            PrivateKey = privateKey;
        }

        public async Task<Cancel> CancelChargeAsync(string id1, string id2, decimal? amount = null)
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
