using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class InvoiceFactoringTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new InvoiceFactoring());
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new InvoiceFactoring());
            var charge = await result.ChargeAsync(10m, "EUR", new Uri("https://www.meinShop.de"), 
                GetMaximumCustomerSameAddress(GetRandomInvoiceId()), FilledBasket());

            Assert.NotNull(result?.Id);
            Assert.NotNull(charge?.Resources?.PaymentId);
        }

        [Fact]
        public async Task Charge_PaymentType_Different_Address()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new InvoiceFactoring());
            var ex = await Assert.ThrowsAsync<PaymentException>(() => result.ChargeAsync(10m, "EUR", new Uri("https://www.meinShop.de"),
                GetMaximumCustomer(GetRandomInvoiceId()), FilledBasket()));
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new InvoiceFactoring());
            var fetched = await BuildHeidelpay().FetchPaymentTypeAsync<InvoiceFactoring>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
