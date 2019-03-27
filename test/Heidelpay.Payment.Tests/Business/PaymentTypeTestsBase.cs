using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.PaymentTypes;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Heidelpay.Payment.Internal.Tests.Business
{
    public abstract class PaymentTypeTestsBase
    {
        protected Heidelpay BuildHeidelpay(string privateKey = null)
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddLogging();

            services.AddHeidelpay(opt =>
            {
                opt.ApiEndpoint = new Uri("https://api.heidelpay.com");
                opt.ApiVersion = "v1";
                opt.ApiKey = privateKey ?? "s-priv-2a102ZMq3gV4I3zJ888J7RR6u75oqK3n";
            });

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<Heidelpay>();
        }

        protected Authorization GetAuthorization(string typeId, string customerId = null, string orderId = null, string metadataId = null, string basketId = null) 
        {
            return new Authorization
            {
                Amount = 10m,
                Currency = "EUR",
                ReturnUrl = new Uri("https://www.heidelpay.com"),
                OrderId = orderId,
                Resources = new Resources
                {
                    TypeId = typeId,
                    CustomerId = customerId,
                    MetadataId = metadataId,
                    BasketId = basketId,
                }
            };
	    }

        protected Card PaymentTypeCard { get; } = new Card { Number = "4444333322221111", ExpiryDate = "03/20", CVC = "123" };
    }
}
