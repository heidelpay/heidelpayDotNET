using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.Options;
using Heidelpay.Payment.PaymentTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
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

            var heidelpay = serviceProvider.GetService<IHeidelpay>();

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

            var heidelpay = serviceProvider.GetService<IHeidelpay>();

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

            var heidelpay = serviceProvider.GetService<IHeidelpay>();

            Assert.NotNull(heidelpay);

            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);

            Assert.NotNull(card);
        }

        [Fact]
        public async Task Heidelpay_Simplest_Usage()
        {
            var heidelpay = new HeidelpayClient("s-priv-2a102ZMq3gV4I3zJ888J7RR6u75oqK3n");
            var authorization = await heidelpay.AuthorizeAsync(decimal.One, "EUR", "s-crd-fm7tifzkqewy", new Uri("https://www.heidelpay.com"));

            Assert.NotNull(authorization?.Id);
        }

        [Fact]
        public async Task Heidelpay_Simple_Usage_Test_With_HttpClient()
        {
            var heidelpay = new HeidelpayClient(new HeidelpayApiOptions
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
        public void AddHeidelpay_Add_HttpClientFactory_If_Not_Added_By_Client()
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
        public void AddHeidelpay_Does_Not_Add_HttpClientFactory_If_Added_By_Client()
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
        public void AddHeidelpay_Does_Not_Add_HttpClientFactory_If_Added_By_Client2()
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
        public async Task LoggingTest()
        {
            var services = new ServiceCollection();

            var logger = new Mock<ILogger<RestClient>>();

            services.AddHttpClient();
            services.AddTransient(x => logger.Object);

            services.AddHeidelpay(opt =>
            {
                opt.ApiEndpoint = new Uri("https://api.heidelpay.com");
                opt.ApiVersion = "v1";
                opt.ApiKey = "s-priv-2a102ZMq3gV4I3zJ888J7RR6u75oqK3n";
            });

            var serviceProvider = services.BuildServiceProvider();

            var heidelpay = serviceProvider.GetService<IHeidelpay>();

            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);

            logger.Verify(m => m.Log(
                It.IsAny<LogLevel>(), 
                It.IsAny<EventId>(),
                It.IsAny<object>(), 
                It.IsAny<TaskCanceledException>(), 
                It.IsAny<Func<object, Exception, string>>()));
        }

        protected Action<Card> PaymentTypeCard { get; } = new Action<Card>(x =>
        {
            x.Number = "4444333322221111";
            x.ExpiryDate = "03/20";
            x.CVC = "123";
        });
    }
}
