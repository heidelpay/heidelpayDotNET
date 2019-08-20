// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-15-2019
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
    internal abstract class ApiServiceBase
    {
        /// <summary>
        /// The heidelpay client instance
        /// </summary>
        protected readonly HeidelpayClient Heidelpay;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentApiService"/> class.
        /// </summary>
        /// <param name="heidelpay">The heidelpay client instance.</param>
        public ApiServiceBase(HeidelpayClient heidelpay)
        {
            Check.ThrowIfNull(heidelpay, nameof(heidelpay));

            this.Heidelpay = heidelpay;
        }

        /// <summary>
        /// ensure rest resource identifier as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> EnsureRestResourceIdAsync<T>(T resource)
            where T : class, IRestResource
        {
            var response = (await Heidelpay.RestClient.HttpPostAsync(BuildUri(resource.GetType()), resource, typeof(IdResponse))) as IdResponse;
            return response?.Id;
        }

        protected async Task<object> ApiGetAsync(Type resourceType, string id)
        {
            var result = await Heidelpay.RestClient.HttpGetAsync(BuildUri(resourceType, id), resourceType);
            return PostProcessApiResource(result);
        }

        /// <summary>
        /// API get as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        protected async Task<T> ApiGetAsync<T>(Uri uri)
             where T : class, IRestResource
        {
            var result = await Heidelpay.RestClient.HttpGetAsync<T>(uri);
            return PostProcessApiResource(result);
        }

        /// <summary>
        /// API get as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        protected async Task<T> ApiGetAsync<T>(string id)
             where T : class, IRestResource
        {
            var result = await Heidelpay.RestClient.HttpGetAsync<T>(BuildUri<T>(id));
            return PostProcessApiResource(result);
        }

        /// <summary>
        /// API post as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="getAfterPost">if set to <c>true</c> [get after post].</param>
        /// <returns>Task&lt;T&gt;.</returns>
        protected async Task<T> ApiPostAsync<T>(T resource, Uri uri = null, bool getAfterPost = true)
           where T : class, IRestResource
        {
            var posted = await Heidelpay.RestClient.HttpPostAsync<T>(uri ?? BuildUri<T>(), resource);
            return getAfterPost
                ? await ApiGetAsync<T>(posted.Id)
                : PostProcessApiResource(posted);
        }

        /// <summary>
        /// API put as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="getAfterPut">if set to <c>true</c> [get after put].</param>
        /// <returns>Task&lt;T&gt;.</returns>
        protected async Task<T> ApiPutAsync<T>(string id, T resource, bool getAfterPut = false)
           where T : class, IRestResource
        {
            var putted = await Heidelpay.RestClient.HttpPutAsync<T>(BuildUri<T>(id), resource);
            return getAfterPut
                ? await ApiGetAsync<T>(putted.Id)
                : PostProcessApiResource(putted);
        }

        /// <summary>
        /// API delete as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        protected async Task ApiDeleteAsync<T>(string id)
           where T : class, IRestResource
        {
            await Heidelpay.RestClient.HttpDeleteAsync<T>(BuildUri<Customer>(id));
        }

        /// <summary>
        /// Builds the URI.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>Uri.</returns>
        protected Uri BuildUri<T>(string id = null)
            where T : class, IRestResource
        {
            return BuildUri(Registry.ResolveResourceUrl<T>(), id);
        }

        /// <summary>
        /// Builds the URI.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Uri.</returns>
        protected Uri BuildUri(Type resourceType, string id = null)
        {
            return BuildUri(Registry.ResolveResourceUrl(resourceType), id);
        }

        /// <summary>
        /// Builds the API endpoint URI.
        /// </summary>
        /// <param name="resourceUrl">The resource URL.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Uri.</returns>
        protected Uri BuildUri(string resourceUrl, string id)
        {
            var rootPath = new Uri(Heidelpay.RestClient?.Options?.ApiEndpoint, Heidelpay.RestClient?.Options?.ApiVersion.EnsureTrailingSlash());
            var combinedPaths = new Uri(rootPath, resourceUrl);

            if (!string.IsNullOrEmpty(id))
            {
                combinedPaths = new Uri(combinedPaths, id.EnsureTrailingSlash());
            }

            return combinedPaths;
        }


        /// <summary>
        /// Postprocess API resource.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <returns>T.</returns>
        private T PostProcessApiResource<T>(T resource)
        {
            if (resource is IHeidelpayProvider provider)
            {
                provider.Heidelpay = Heidelpay;
            }

            return resource;
        }

        /// <summary>
        /// Class IdResponse.
        /// Implements the <see cref="Heidelpay.Payment.Interfaces.IRestResource" />
        /// </summary>
        /// <seealso cref="Heidelpay.Payment.Interfaces.IRestResource" />
        internal class IdResponse : IRestResource
        {
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            public string Id { get; set; }
        }
    }
}
