using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.Options;
using Heidelpay.Payment.Tests.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.Tests
{
    public class SdkUsageTests
    {
        [Fact]
        public async Task Heidelpay_DI_Usage_Test()
        {
            var services = new ServiceCollection();

            var configBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true);

            var config = configBuilder.Build();

            services.AddHttpClient();
            services.AddLogging();

            services.AddHeidelpay(config.GetSection("Heidelpay"));

            var serviceProvider = services.BuildServiceProvider();

            var heidelpay = serviceProvider.GetService<Heidelpay>();

            Assert.NotNull(heidelpay);
            Assert.NotNull(heidelpay.RestClient);
            Assert.NotNull(heidelpay.Options);
        }

        [Fact]
        public async Task Heidelpay_Simple_Usage_Test()
        {
            var heidelpay = new Heidelpay(new HeidelpayApiOptions
            {
                ApiEndpoint = new Uri("https://api.heidelpay.com"),
                ApiKey = "Samplekey",
            });

            Assert.NotNull(heidelpay);
            Assert.NotNull(heidelpay.RestClient);
            Assert.NotNull(heidelpay.Options);
        }
    }
}
