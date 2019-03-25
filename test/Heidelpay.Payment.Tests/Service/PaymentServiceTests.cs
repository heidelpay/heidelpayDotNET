using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.Options;
using Heidelpay.Payment.Service;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.Tests.Service
{
    public class PaymentServiceTests
    {
        [Fact]
        public async Task Charge_In_Case_Of_Core_Exception()
        {
            var mockedRestClient = new Mock<IRestClient>();

            var error = JsonConvert.DeserializeObject<RestClientErrorObject>(ErrorJson());

            mockedRestClient
                .SetupGet(x => x.Options)
                .Returns(new HeidelpayApiOptions { ApiKey = "asd", ApiEndpoint = new Uri("https://heidelpay.com"), ApiVersion = "v1" });

            mockedRestClient
                .Setup(x => x.HttpPostAsync<Charge>(It.IsAny<Uri>(), It.IsAny<Charge>()))
                .Throws(HttpResponseExtensions.AsException(error, HttpStatusCode.InternalServerError));

            var heidelpay = new Heidelpay(mockedRestClient.Object);

            var exception = await Assert.ThrowsAsync<PaymentException>(
                () => heidelpay.PaymentService.ChargeAsync(new Charge()));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var paymentError = exception.PaymentErrorList.First();

            Assert.Equal("COR.400.100.101", paymentError.Code);
            Assert.Equal("Address untraceable", paymentError.MerchantMessage);
            Assert.Equal("The provided address is invalid. Please check your input and try agian.", paymentError.CustomerMessage);
        }

        private static string ErrorJson()
        {
            return "{" +
                    "  \"id\" : \"s-err-f2ea241e5e8e4eb3b1513fab12c\"," +
                    "  \"url\" : \"https://api.heidelpay.com/v1/payments/charges\"," +
                    "  \"timestamp\" : \"2019-01-09 15:42:24\"," +
                    "  \"errors\" : [ {\r\n" +
                    "    \"code\" : \"COR.400.100.101\"," +
                    "    \"merchantMessage\" : \"Address untraceable\",\r\n" +
                    "    \"customerMessage\" : \"The provided address is invalid. Please check your input and try agian.\"," +
                    "    \"status\" : {" +
                    "      \"successful\" : false," +
                    "      \"processing\" : false," +
                    "      \"pending\" : false" +
                    "    }" +
                    "  } ]" +
                    "}";
        }
    }
}
