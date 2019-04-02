// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="ServiceCollectionExtensions.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;

namespace Heidelpay.Payment.Extensions
{
    /// <summary>
    /// Class ServiceCollectionExtensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the heidelpay.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="setupAction">The setup action.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddHeidelpay(this IServiceCollection serviceCollection,
            Action<HeidelpayApiOptions> setupAction)
        {
            Check.NotNull(serviceCollection, (nameof(serviceCollection)));
            Check.NotNull(setupAction, (nameof(setupAction)));

            serviceCollection.Configure(setupAction);

            return AddHeidelpay(serviceCollection);
        }

        /// <summary>
        /// Adds the heidelpay.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddHeidelpay(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            Check.NotNull(serviceCollection, (nameof(serviceCollection)));
            Check.NotNull(configuration, (nameof(configuration)));

            serviceCollection.Configure<HeidelpayApiOptions>(configuration);

            return AddHeidelpay(serviceCollection);
        }

        /// <summary>
        /// Adds the heidelpay.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>IServiceCollection.</returns>
        private static IServiceCollection AddHeidelpay(this IServiceCollection serviceCollection)
        {
            if(!serviceCollection.Any(x => x.ServiceType == typeof(IHttpClientFactory)))
            {
                serviceCollection.AddHttpClient();
            }

            return serviceCollection
                .AddTransient<IRestClient, RestClient>(sp => new RestClient(
                    sp.GetRequiredService<IHttpClientFactory>(), 
                    sp.GetRequiredService<IOptions<HeidelpayApiOptions>>(),
                    sp.GetService<ILogger<RestClient>>()))
                .AddTransient<IHeidelpay>(sp => new HeidelpayClient(sp.GetRequiredService<IRestClient>()));
        }
    }
}
