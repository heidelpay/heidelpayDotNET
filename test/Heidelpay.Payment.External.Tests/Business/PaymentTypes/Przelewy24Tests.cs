using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class Przelewy24Tests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<Przelewy24>();
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<Przelewy24>();
            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "PLN", result, TestReturnUri);
            Assert.NotNull(charge?.Id);
            Assert.NotNull(charge?.RedirectUrl);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<Przelewy24>();
            var fetched = await BuildHeidelpay().FetchPaymentTypeAsync<Przelewy24>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
