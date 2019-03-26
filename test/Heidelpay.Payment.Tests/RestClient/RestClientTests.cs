using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.Tests.Communication
{
    public class RestClientTests
    {
        readonly Uri TestUri = new Uri("https://heidelpay.com");
        const string PrivateKey = "Samplekey";

        const string ErrorResponse = 
                "{" +
                "    \"url\": \"https://heidelpay.com\"," +
                "    \"timestamp\": \"2018-09-13 22:47:35\"," +
                "    \"errors\": [" +
                "        {" +
                "            \"code\": \"API.410.200.010\"," +
                "            \"customerMessage\": \"Message for the customer.\"," +
                "			 \"merchantMessage\": \"Message for the merchant.\"" +
                "        }" +
                "    ]" +
                "}";

        const string ValidResponse = "{\"key\": \"value\"}";

        private MockRestClient BuildMockRestClient(string response, HttpStatusCode code = HttpStatusCode.OK)
        {
            var httpClientName = "MockHttpMessageHandler";

            var services = new ServiceCollection();

            services.Configure<HeidelpayApiOptions>(opt =>
            {
                opt.HttpClientName = httpClientName;
                opt.ApiEndpoint = TestUri;
                opt.ApiVersion = "v1";
                opt.ApiKey = PrivateKey;
            });

            services
                .AddHttpClient<HttpClient>(httpClientName)
                .ConfigurePrimaryHttpMessageHandler(() => new MockHttpMessageHandler(code, response));
            services.AddLogging();

            var serviceProvider = services.BuildServiceProvider();

            var factory = serviceProvider.GetService<IHttpClientFactory>();
            var logger = serviceProvider.GetService<ILogger<RestClient>>();
            var options = serviceProvider.GetService<IOptions<HeidelpayApiOptions>>();

            return new MockRestClient(factory, options, logger);
        }

        [Fact]
        public async Task Api_Errors_Are_Translated_To_Payment_Exception()
        {
            var restClient = BuildMockRestClient(response: ErrorResponse, code: HttpStatusCode.Conflict);

            var exception = await Assert.ThrowsAsync<PaymentException>(
                () => restClient.HttpGetAsync<ContentTestData>(TestUri));

            Assert.NotNull(exception);
            Assert.Equal(DateTime.Parse("2018-09-13 22:47:35"), exception.Timestamp);
            Assert.Equal(TestUri, exception.Uri);
            Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.410.200.010", error.Code);
            Assert.Equal("Message for the customer.", error.CustomerMessage);
            Assert.Equal("Message for the merchant.", error.MerchantMessage);
        }

        [Fact]
        public async Task BasicHttpGet()
        {
            var restClient = BuildMockRestClient(response: ValidResponse);

            var response = await restClient.HttpGetAsync<ValidResponseClass>(TestUri);

            Assert.Equal(JsonConvert.DeserializeObject<ValidResponseClass>(ValidResponse), response);
            Assert.Equal(HttpStatusCode.OK, restClient.LoggedResponse.StatusCode);
        }

        [Fact]
        public async Task BasicHttpPost()
        {
            var testContentData = new ContentTestData
            {
                TestProperty1 = 1,
                TestProperty2 = "BasicHttpPost",
                TestProperty3 = 1.23m,
                TestProperty4 = DateTime.Now,
            };

            var restClient = BuildMockRestClient(response: ValidResponse, code: HttpStatusCode.Created);

            var response = await restClient.HttpPostAsync<ValidResponseClass>(TestUri, testContentData);

            Assert.Equal(JsonConvert.DeserializeObject<ValidResponseClass>(ValidResponse), response);
            Assert.Equal(HttpStatusCode.Created, restClient.LoggedResponse.StatusCode);
            Assert.Equal(testContentData, JsonConvert.DeserializeObject<ContentTestData>(
                await restClient.LoggedRequest.Content.ReadAsStringAsync()));
        }

        [Fact]
        public async Task BasicHttpPut()
        {
            var testContentData = new ContentTestData
            {
                TestProperty1 = 2,
                TestProperty2 = "BasicHttpPut",
                TestProperty3 = 3.45m,
                TestProperty4 = DateTime.Now,
            };

            var restClient = BuildMockRestClient(response: ValidResponse);

            var response = await restClient.HttpPutAsync<ValidResponseClass>(TestUri, testContentData);

            Assert.Equal(JsonConvert.DeserializeObject<ValidResponseClass>(ValidResponse), response);
            Assert.Equal(HttpStatusCode.OK, restClient.LoggedResponse.StatusCode);
            Assert.Equal(testContentData, JsonConvert.DeserializeObject<ContentTestData>(
                await restClient.LoggedRequest.Content.ReadAsStringAsync()));
        }

        [Fact]
        public async Task BasicHttpDelete()
        {
            var restClient = BuildMockRestClient(response: ValidResponse);

            var response = await restClient.HttpDeleteAsync<ValidResponseClass>(TestUri);

            Assert.Equal(JsonConvert.DeserializeObject<ValidResponseClass>(ValidResponse), response);
            Assert.Equal(HttpStatusCode.OK, restClient.LoggedResponse.StatusCode);
        }

        [Fact]
        public async Task Auth_And_UserAgent_Header_Are_Set_On_Get_Request()
        {
            var restClient = BuildMockRestClient(response: ValidResponse);

            var _ = await restClient.HttpGetAsync<ValidResponseClass>(TestUri);

            AssertUserAgentHeader(restClient);
            AssertAuthentication(restClient);
        }

        [Fact]
        public async Task Auth_ContentType_And_UserAgent_Header_And_Bodies_Content_Encoding_Are_Set_On_Post_Request()
        {
            var restClient = BuildMockRestClient(response: ValidResponse);

            var _ = await restClient.HttpPostAsync<ValidResponseClass>(TestUri, new ContentTestData());

            AssertUserAgentHeader(restClient);
            AssertAuthentication(restClient);
            AssertContentTypeHeader(restClient);
            AssertContentBodyEncoding(restClient);
        }

        [Fact]
        public async Task Auth_ContentType_And_UserAgent_Header_And_Bodies_Content_Encoding_Are_Set_On_Put_Request()
        {
            var restClient = BuildMockRestClient(response: ValidResponse);

            var _ = await restClient.HttpPutAsync<ValidResponseClass>(TestUri, new ContentTestData());

            AssertUserAgentHeader(restClient);
            AssertAuthentication(restClient);
            AssertContentTypeHeader(restClient);
            AssertContentBodyEncoding(restClient);
        }

        [Fact]
        public async Task Auth_And_UserAgent_Header_Are_Set_On_Delete_Request()
        {
            var restClient = BuildMockRestClient(ValidResponse);

            var _ = await restClient.HttpDeleteAsync<ValidResponseClass>(TestUri);

            AssertUserAgentHeader(restClient);
            AssertAuthentication(restClient);
        }

        private static void AssertContentBodyEncoding(MockRestClient restClient)
        {
            Assert.Equal("UTF-8",
                restClient.LoggedRequest.Content.Headers.ContentEncoding.ToString());
        }

        private static void AssertContentTypeHeader(MockRestClient restClient)
        {
            Assert.Equal("application/json; charset=UTF-8",
                restClient.LoggedRequest.Content.Headers.ContentType.ToString(), ignoreCase: true, ignoreWhiteSpaceDifferences: true);
        }

        private static void AssertAuthentication(MockRestClient restClient)
        {
            Assert.Equal("Basic " + CoreExtensions.EncodeToBase64(PrivateKey + ":"),
                restClient.LoggedRequest.Headers.Authorization.ToString());
        }

        private static void AssertUserAgentHeader(MockRestClient restClient)
        {
            Assert.Equal($"{RestClientConstants.USER_AGENT_PREFIX}{SDKInfo.Version} - {typeof(MockRestClient).FullName}",
                restClient.LoggedRequest.Headers.UserAgent.ToString());
        }

        class ContentTestData
        {
            public int TestProperty1 { get; set; }
            public string TestProperty2 { get; set; }
            public decimal TestProperty3 { get; set; }
            public DateTime TestProperty4 { get; set; }

            public override bool Equals(object obj)
            {
                var other = (ContentTestData)obj;

                return
                    this.TestProperty1 == other.TestProperty1 &&
                    this.TestProperty2 == other.TestProperty2 &&
                    this.TestProperty3 == other.TestProperty3 &&
                    this.TestProperty4 == other.TestProperty4;
            }
        }

        class ValidResponseClass
        {
            public string Key { get; set; }

            public override bool Equals(object obj)
            {
                return obj is ValidResponseClass && ((ValidResponseClass)obj).Key == Key;
            }
        }
    }
}
