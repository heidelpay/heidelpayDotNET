using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.Internal.Tests.Business.Errors
{
    public class ErrorTests
    {
        protected IHeidelpay BuildHeidelpay(string privateKey = null)
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

            return serviceProvider.GetService<IHeidelpay>();
        }

        [Fact]
        public async Task PaymentTypeId_Missing()
        {
            var heidelpay = BuildHeidelpay();

            var exception = await Assert.ThrowsAsync<PaymentException>(() => heidelpay.AuthorizeAsync(new Authorization { Amount = 10m, Currency = "EUR", ReturnUrl = new Uri("https://www.google.at") }));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.320.200.143", error.Code);
            Assert.Equal("Resources type id is missing", error.MerchantMessage);
        }
    }
}
