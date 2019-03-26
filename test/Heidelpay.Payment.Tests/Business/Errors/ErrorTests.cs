using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.Tests.Business.Errors
{
    public class ErrorTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Key_Missing()
        {
            var heidelpay = BuildHeidelpay("");
            var exception = await Assert.ThrowsAsync<PaymentException>(
                () => heidelpay.AuthorizeAsync(GetAuthorization("")));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.000.000.001", error.Code);
            Assert.Equal("PrivateKey/PublicKey is missing", error.MerchantMessage);
        }
    }
}
