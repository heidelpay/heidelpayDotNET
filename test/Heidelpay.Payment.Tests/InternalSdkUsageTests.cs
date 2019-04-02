using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.IO;
using System.Net.Http;
using Xunit;

namespace Heidelpay.Payment.Internal.Tests
{
    public class InternalSdkUsageTests
    {
        [Fact]
        public void Heidelpay_DI_Usage_Test_With_User_Setup()
        {
            var services = new ServiceCollection();

            var configBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.internal.json", optional: true);

            var config = configBuilder.Build();

            services.AddHttpClient();
            services.AddLogging();

            services.AddHeidelpay(config.GetSection("Heidelpay"));

            var serviceProvider = services.BuildServiceProvider();

            var heidelpay = serviceProvider.GetService<HeidelpayClient>();

            Assert.NotNull(heidelpay);
            Assert.NotNull(heidelpay.RestClient);
            Assert.NotNull(heidelpay.RestClient.Options);
            Assert.NotNull(heidelpay.PaymentService);

            Assert.Equal("SamplekeyFromFile", heidelpay.RestClient.Options.ApiKey);
            Assert.Equal("https://api.heidelpay.com/", heidelpay.RestClient.Options.ApiEndpoint.ToString());
            Assert.Null(heidelpay.RestClient.Options.HttpClientName);
        }

        [Fact]
        public void Heidelpay_Simple_Usage_Test_With_HttpClient()
        {
            var heidelpay = new HeidelpayClient(new HeidelpayApiOptions
            {
                ApiEndpoint = new Uri("https://api.heidelpay.com"),
                ApiVersion = "v1",
                ApiKey = "Samplekey2",
            }, new HttpClient());

            Assert.NotNull(heidelpay);
            Assert.NotNull(heidelpay.RestClient);
            Assert.NotNull(heidelpay.RestClient.Options);
            Assert.NotNull(heidelpay.PaymentService);

            Assert.Equal("Samplekey2", heidelpay.RestClient.Options.ApiKey);
            Assert.Equal("https://api.heidelpay.com/", heidelpay.RestClient.Options.ApiEndpoint.ToString());
            Assert.Null(heidelpay.RestClient.Options.HttpClientName);
        }

        [Fact]
        public void Heidelpay_Simple_Usage_Test_With_HttpClientFactory()
        {
            var factoryMock = new Mock<IHttpClientFactory>();

            factoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());

            var heidelpay = new HeidelpayClient(new HeidelpayApiOptions
            {
                ApiEndpoint = new Uri("https://api.heidelpay.com"),
                ApiVersion = "v1",
                ApiKey = "Samplekey1",
            }, factoryMock.Object);

            Assert.NotNull(heidelpay);
            Assert.NotNull(heidelpay.RestClient);
            Assert.NotNull(heidelpay.RestClient.Options);
            Assert.NotNull(heidelpay.PaymentService);

            Assert.Equal("Samplekey1", heidelpay.RestClient.Options.ApiKey);
            Assert.Equal("https://api.heidelpay.com/", heidelpay.RestClient.Options.ApiEndpoint.ToString());
            Assert.Null(heidelpay.RestClient.Options.HttpClientName);
        }
    }
}
