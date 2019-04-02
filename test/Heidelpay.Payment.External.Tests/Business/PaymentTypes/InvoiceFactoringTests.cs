using Heidelpay.Payment.PaymentTypes;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class InvoiceFactoringTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<InvoiceFactoring>();
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<InvoiceFactoring>();
            var charge = await result.ChargeAsync(10m, "EUR", ShopReturnUri, 
                GetMaximumCustomerSameAddress(GetRandomInvoiceId()), FilledBasket());

            Assert.NotNull(result?.Id);
            Assert.NotNull(charge?.PaymentId);
        }

        [Fact]
        public async Task Charge_PaymentType_Different_Address()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<InvoiceFactoring>();
            var ex = await Assert.ThrowsAsync<PaymentException>(() => result.ChargeAsync(10m, "EUR", ShopReturnUri,
                GetMaximumCustomer(GetRandomInvoiceId()), FilledBasket()));
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<InvoiceFactoring>();
            var fetched = await BuildHeidelpay().FetchPaymentTypeAsync<InvoiceFactoring>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
