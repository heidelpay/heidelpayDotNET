using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.Options;
using Heidelpay.Payment.Tests.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
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
        public async Task Heidelpay_DI_Usage_Test_With_User_Setup()
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
            Assert.NotNull(heidelpay.RestClient.Options);

            Assert.Equal("SamplekeyFromFile", heidelpay.RestClient.Options.ApiKey);
            Assert.Equal("https://api.heidelpay.com/", heidelpay.RestClient.Options.ApiEndpoint.ToString());
            Assert.Null(heidelpay.RestClient.Options.HttpClientName);
        }

        [Fact]
        public async Task Heidelpay_DI_Usage_Test_Without_User_Setup()
        {
            var services = new ServiceCollection();

            var configBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true);

            var config = configBuilder.Build();

            services.AddHeidelpay(config.GetSection("Heidelpay"));

            var serviceProvider = services.BuildServiceProvider();

            Assert.Throws<InvalidOperationException>(() => _ = serviceProvider.GetService<Heidelpay>());
        }

        [Fact]
        public async Task Heidelpay_Simple_Usage_Test()
        {
            var heidelpay = new Heidelpay(new HeidelpayApiOptions
            {
                ApiEndpoint = new Uri("https://api.heidelpay.com"),
                ApiKey = "Samplekey3",
            });

            Assert.NotNull(heidelpay);
            Assert.NotNull(heidelpay.RestClient);
            Assert.NotNull(heidelpay.RestClient.Options);

            Assert.Equal("Samplekey3", heidelpay.RestClient.Options.ApiKey);
            Assert.Equal("https://api.heidelpay.com/", heidelpay.RestClient.Options.ApiEndpoint.ToString());
            Assert.Null(heidelpay.RestClient.Options.HttpClientName);
        }

        [Fact]
        public async Task Heidelpay_Simple_Usage_Test_With_HttpClient()
        {
            var heidelpay = new Heidelpay(new HeidelpayApiOptions
            {
                ApiEndpoint = new Uri("https://api.heidelpay.com"),
                ApiKey = "Samplekey2",
            }, new HttpClient());

            Assert.NotNull(heidelpay);
            Assert.NotNull(heidelpay.RestClient);
            Assert.NotNull(heidelpay.RestClient.Options);

            Assert.Equal("Samplekey2", heidelpay.RestClient.Options.ApiKey);
            Assert.Equal("https://api.heidelpay.com/", heidelpay.RestClient.Options.ApiEndpoint.ToString());
            Assert.Null(heidelpay.RestClient.Options.HttpClientName);
        }

        [Fact]
        public async Task Heidelpay_Simple_Usage_Test_With_HttpClientFactory()
        {
            var factoryMock = new Mock<IHttpClientFactory>();

            factoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());

            var heidelpay = new Heidelpay(new HeidelpayApiOptions
            {
                ApiEndpoint = new Uri("https://api.heidelpay.com"),
                ApiKey = "Samplekey1",
            }, factoryMock.Object);

            Assert.NotNull(heidelpay);
            Assert.NotNull(heidelpay.RestClient);
            Assert.NotNull(heidelpay.RestClient.Options);

            Assert.Equal("Samplekey1", heidelpay.RestClient.Options.ApiKey);
            Assert.Equal("https://api.heidelpay.com/", heidelpay.RestClient.Options.ApiEndpoint.ToString());
            Assert.Null(heidelpay.RestClient.Options.HttpClientName);
        }
    }
}
