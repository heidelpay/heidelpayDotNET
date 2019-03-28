using Heidelpay.Payment.PaymentTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class CardTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_Card_With_Merchant_Not_PCIDSS_Compliant()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(TestCard);
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
        }

        [Fact]
        public async Task Authorize_Card_Type()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(TestCard);
            var authorization = await card.AuthorizeAsync(decimal.One, "EUR", new Uri("https://www.meinShop.de"));
            Assert.NotNull(authorization);
            Assert.NotNull(authorization.Id);
        }

        private static Card TestCard { get; } = new Card { Number = "4444333322221111", ExpiryDate = "03/20", CVC = "123" };
    }
}
