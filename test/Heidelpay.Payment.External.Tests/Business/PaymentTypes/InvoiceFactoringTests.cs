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
            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceFactoring>();
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceFactoring>();
            var charge = await result.ChargeAsync(10m, "EUR", ShopReturnUri, 
                GetMaximumCustomerSameAddress(GetRandomInvoiceId()), GetMaximumBasket());

            AssertCharge(charge, 10m);
            Assert.NotNull(charge?.PaymentId);
        }

        [Fact]
        public async Task Charge_PaymentType_Different_Address()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceFactoring>();
            var ex = await Assert.ThrowsAsync<PaymentException>(() => result.ChargeAsync(10m, "EUR", ShopReturnUri,
                GetMaximumCustomer(GetRandomInvoiceId()), GetMaximumBasket()));
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceFactoring>();
            var fetched = await Heidelpay.FetchPaymentTypeAsync<InvoiceFactoring>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
