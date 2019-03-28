using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class PaypalTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Paypal());
            Assert.NotNull(result?.Id);
        }


        [Fact]
        public async Task Authorize_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Paypal());
            var auth = await BuildHeidelpay().AuthorizeAsync(decimal.One, "EUR", result, new Uri("https://www.meinShop.de"));
            Assert.NotNull(result?.Id);
            Assert.NotNull(auth?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Paypal());
            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "EUR", result, new Uri("https://www.google.at"));
            Assert.NotNull(charge?.Id);
            Assert.NotNull(charge?.RedirectUrl);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Paypal());
            var fetched = await BuildHeidelpay().FetchPaymentTypeAsync<Paypal>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
