using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Options;
using Heidelpay.Payment.PaymentTypes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Tests.Business
{
    public abstract class PaymentTypeTestBase
    {
        protected Heidelpay BuildHeidelpay()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddLogging();

            services.AddHeidelpay(opt =>
            {
                opt.ApiEndpoint = new Uri("https://api.heidelpay.com");
                opt.ApiVersion = "v1";
                opt.ApiKey = "s-priv-2a102ZMq3gV4I3zJ888J7RR6u75oqK3n";
            });

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<Heidelpay>();
        }

        protected async Task<Card> CreatePaymentTypeCard(Heidelpay heidelpay)
        {
            return await heidelpay.PaymentService.CreatePaymentTypeBaseAsync(PaymentTypeCard);
        }

        protected Card PaymentTypeCard { get; } = new Card { Number = "4444333322221111", ExpiryDate = "03/20", CVC = "123" };
    }
}
