using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class InvoiceTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Invoice());
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Invoice());
            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "EUR", result, ShopReturnUri);
            Assert.NotNull(charge?.Id);
            Assert.NotNull(charge?.ReturnUrl);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Invoice());
            var fetched = await BuildHeidelpay().FetchPaymentTypeAsync<Invoice>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
