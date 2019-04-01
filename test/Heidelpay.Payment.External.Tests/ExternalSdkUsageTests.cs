using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Options;
using Heidelpay.Payment.PaymentTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests
{
    public class ExternalSdkUsageTests
    {
        [Fact]
        public async Task Heidelpay_DI_Usage_Test_With_Settings_File()
        {
            var services = new ServiceCollection();

            var configBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.external.json", optional: true);

            var config = configBuilder.Build();

            services.AddHeidelpay(config.GetSection("Heidelpay"));

            var serviceProvider = services.BuildServiceProvider();

            var heidelpay = serviceProvider.GetService<Heidelpay>();

            Assert.NotNull(heidelpay);

            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);

            Assert.NotNull(card);
        }

        [Fact]
        public async Task Heidelpay_DI_Usage_Test_With_User_Setup()
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

            var heidelpay = serviceProvider.GetService<Heidelpay>();

            Assert.NotNull(heidelpay);

            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);

            Assert.NotNull(card);
        }

        [Fact]
        public async Task Heidelpay_DI_Usage_Test_Without_User_Setup()
        {
            var services = new ServiceCollection();

            var configBuilder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.external.json", optional: true);

            var config = configBuilder.Build();

            services.AddHeidelpay(config.GetSection("Heidelpay"));

            var serviceProvider = services.BuildServiceProvider();

            var heidelpay = serviceProvider.GetService<Heidelpay>();

            Assert.NotNull(heidelpay);

            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);

            Assert.NotNull(card);
        }

        [Fact]
        public async Task Heidelpay_Simple_Usage_Test_With_HttpClient()
        {
            var heidelpay = new Heidelpay(new HeidelpayApiOptions
            {
                ApiEndpoint = new Uri("https://api.heidelpay.com"),
                ApiVersion = "v1",
                ApiKey = "s-priv-2a102ZMq3gV4I3zJ888J7RR6u75oqK3n"
            }, new HttpClient());

            Assert.NotNull(heidelpay);

            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);

            Assert.NotNull(card);
        }

        [Fact]
        public async Task AddHeidelpay_Add_HttpClientFactory_If_Not_Added_By_Client()
        {
            var services = new ServiceCollection();

            services.AddHeidelpay(x =>
            {
                x.ApiEndpoint = new Uri("https://api.heidelpay.com");
                x.ApiVersion = "v1";
                x.ApiKey = "Samplekey1";
            });

            var provider = services.BuildServiceProvider();

            var factory = provider.GetService<IHttpClientFactory>();

            Assert.NotNull(factory);
        }

        [Fact]
        public async Task AddHeidelpay_Does_Not_Add_HttpClientFactory_If_Added_By_Client()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();

            services.AddHeidelpay(x =>
            {
                x.ApiEndpoint = new Uri("https://api.heidelpay.com");
                x.ApiVersion = "v1";
                x.ApiKey = "Samplekey1";
            });

            var provider = services.BuildServiceProvider();

            var factory = provider.GetService<IHttpClientFactory>();

            Assert.NotNull(factory);
        }

        [Fact]
        public async Task AddHeidelpay_Does_Not_Add_HttpClientFactory_If_Added_By_Client2()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();

            services.AddHeidelpay(x =>
            {
                x.ApiEndpoint = new Uri("https://api.heidelpay.com");
                x.ApiVersion = "v1";
                x.ApiKey = "Samplekey1";
            });

            var provider = services.BuildServiceProvider();

            var factory = provider.GetService<IHttpClientFactory>();

            Assert.NotNull(factory);
        }

        protected Card PaymentTypeCard { get; } = new Card { Number = "4444333322221111", ExpiryDate = "03/20", CVC = "123" };
    }
}
