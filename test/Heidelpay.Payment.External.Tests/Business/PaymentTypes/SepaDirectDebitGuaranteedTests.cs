using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class SepaDirectDebitGuaranteedTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<SepaDirectDebitGuaranteed>(x => x.Iban = "DE89370400440532013000");
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Create_PaymentType_Full()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(TestPaymentType);
            Assert.NotNull(result?.Id);

            Assert.Equal("COBADEFFXXX", result.Bic);
            Assert.Equal("Max Musterperson", result.Holder);
            Assert.Equal("DE89370400440532013000", result.Iban);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(TestPaymentType);
            var fetched = await Heidelpay.FetchPaymentTypeAsync<SepaDirectDebitGuaranteed>(result.Id);
            Assert.NotNull(fetched?.Id);

            Assert.Equal("COBADEFFXXX", fetched.Bic);
            Assert.Equal("Max Musterperson", fetched.Holder);
            //Assert.Equal("DE8937************3000", result.Iban);
            Assert.Equal("DE89370400440532013000", result.Iban);
        }

        private Action<SepaDirectDebitGuaranteed> TestPaymentType { get; } = new Action<SepaDirectDebitGuaranteed>(x =>
        {
            x.Iban = "DE89370400440532013000";
            x.Bic = "COBADEFFXXX";
            x.Holder = "Max Musterperson";
        });
    }
}
