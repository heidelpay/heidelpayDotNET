using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class SepaDirectTransitGuaranteedTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new SepaDirectDebitGuaranteed { Iban = "DE89370400440532013000" });
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Create_PaymentType_Full()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(TestPaymentType);
            Assert.NotNull(result?.Id);

            AssertEqual(TestPaymentType, result);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(TestPaymentType);
            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "EUR", result, new Uri("https://www.google.at"));
            Assert.NotNull(charge?.Id);
            Assert.NotNull(charge?.RedirectUrl);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(TestPaymentType);
            var fetched = await BuildHeidelpay().FetchPaymentTypeAsync<SepaDirectDebitGuaranteed>(result.Id);
            Assert.NotNull(fetched?.Id);

            AssertEqual(TestPaymentType, fetched);
        }

        private SepaDirectDebitGuaranteed TestPaymentType { get; } = new SepaDirectDebitGuaranteed { Iban = "DE89370400440532013000", Bic = "COBADEFFXXX", Holder = "Max Musterperson" };

        private static void AssertEqual(SepaDirectDebitGuaranteed expected, SepaDirectDebitGuaranteed actual)
        {
            Assert.NotNull(actual);
            Assert.Equal(expected.Bic, actual.Bic);
            Assert.Equal(expected.Holder, actual.Holder);
            Assert.Equal("DE8937************3000", actual.Iban);
        }
    }
}
