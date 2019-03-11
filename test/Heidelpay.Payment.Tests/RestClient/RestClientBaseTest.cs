using Heidelpay.Payment.RestClient;
using Heidelpay.Payment.Tests.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.Tests
{
    public class RestClientBaseTest
    {
        private string privateKey = "samplekey";

        private IRestClient BuildMockRestClient()
        {
            var services = new ServiceCollection();

            var configBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true);

            var config = configBuilder.Build();

            services.AddHttpClient();
            services.AddLogging();
            services.Configure<SDKOptions>(config.GetSection("Heidelpay"));

            var serviceProvider = services.BuildServiceProvider();

            var factory = serviceProvider.GetService<IHttpClientFactory>();
            var options = serviceProvider.GetService<IOptions<SDKOptions>>();
            var logger = serviceProvider.GetService<ILogger<RestClientBase>>();

            return new MockRestClient(factory, options, logger);
        }

        [Fact]
        public async Task Test_httpGet()
        {
            var restClient = BuildMockRestClient();
            var response = await restClient.HttpGetAsync(new Uri("http://heidelpay.com"), privateKey);
        }
    }
}
