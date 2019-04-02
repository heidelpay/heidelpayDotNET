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
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<Paypal>();
            Assert.NotNull(result?.Id);
        }


        [Fact]
        public async Task Authorize_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<Paypal>();
            var auth = await BuildHeidelpay().AuthorizeAsync(decimal.One, "EUR", result, ShopReturnUri);
            Assert.NotNull(result?.Id);
            Assert.NotNull(auth?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<Paypal>();
            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "EUR", result, TestReturnUri);
            Assert.NotNull(charge?.Id);
            Assert.NotNull(charge?.RedirectUrl);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<Paypal>();
            var fetched = await BuildHeidelpay().FetchPaymentTypeAsync<Paypal>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
