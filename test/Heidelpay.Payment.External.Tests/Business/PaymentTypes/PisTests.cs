using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class PisTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Pis());
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Pis());
            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "EUR", result, new Uri("https://www.google.at"));
            Assert.NotNull(charge?.Id);
            Assert.NotNull(charge?.RedirectUrl);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Pis());
            var fetched = await BuildHeidelpay().FetchPaymentTypeAsync<Pis>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
