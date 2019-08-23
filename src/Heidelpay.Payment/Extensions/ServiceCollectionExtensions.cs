// ***********************************************************************
// Assembly         : Heidelpay.Payment

// ***********************************************************************
// <copyright file="ServiceCollectionExtensions.cs" company="Heidelpay">
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Heidelpay.Payment.Extensions
{
    /// <summary>
    /// Class ServiceCollectionExtensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the heidelpay client default implementation.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="setupAction">The setup action.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddHeidelpay(this IServiceCollection serviceCollection,
            Action<HeidelpayApiOptions> setupAction)
        {
            Check.ThrowIfNull(serviceCollection, (nameof(serviceCollection)));
            Check.ThrowIfNull(setupAction, (nameof(setupAction)));

            serviceCollection.Configure(setupAction);

            return AddHeidelpay(serviceCollection);
        }

        /// <summary>
        /// Adds the heidelpay client default implementation.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddHeidelpay(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            Check.ThrowIfNull(serviceCollection, (nameof(serviceCollection)));
            Check.ThrowIfNull(configuration, (nameof(configuration)));

            serviceCollection.Configure<HeidelpayApiOptions>(configuration);

            return AddHeidelpay(serviceCollection);
        }

        /// <summary>
        /// Adds the heidelpay client default implementation.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>IServiceCollection.</returns>
        private static IServiceCollection AddHeidelpay(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddHttpClient<IRestClient, RestClient>();

            return serviceCollection
                .AddTransient<IHeidelpay, HeidelpayClient>(sp => new HeidelpayClient(sp.GetRequiredService<IRestClient>()));
        }
    }
}
