using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class EpsTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_Eps()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Eps());
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Create_Eps_With_BicType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Eps { Bic = "SPFKAT2BXXX" });
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_Eps_Type()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Eps());
            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "EUR", result, new Uri("https://www.google.at"));
            Assert.NotNull(charge?.Id);
            Assert.NotNull(charge?.RedirectUrl);
        }

        [Fact]
        public async Task Fetch_Eps_Type()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Eps());
            var fetchedEps = await BuildHeidelpay().FetchPaymentTypeAsync<Eps>(result.Id);
            Assert.NotNull(fetchedEps?.Id);
        }
    }
}
