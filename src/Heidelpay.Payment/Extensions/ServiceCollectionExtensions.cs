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
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHeidelpay(this IServiceCollection serviceCollection,
            Action<HeidelpayApiOptions> setupAction)
        {
            Check.NotNull(serviceCollection, (nameof(serviceCollection)));
            Check.NotNull(setupAction, (nameof(setupAction)));

            serviceCollection.Configure(setupAction);

            return AddHeidelpay(serviceCollection);
        }

        public static IServiceCollection AddHeidelpay(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            Check.NotNull(serviceCollection, (nameof(serviceCollection)));
            Check.NotNull(configuration, (nameof(configuration)));

            serviceCollection.Configure<HeidelpayApiOptions>(configuration);

            return AddHeidelpay(serviceCollection);
        }

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
                .AddTransient(sp => new Heidelpay(sp.GetRequiredService<IRestClient>()));
        }
    }
}
