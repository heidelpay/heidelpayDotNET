using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class InvoiceFactoringTests : PaymentTypeTestsBase
    {
        private Action<InvoiceFactoring> ConfigurePaymentType { get; } = new Action<InvoiceFactoring>(x =>
        {

        });

        [Fact]
        public async Task Create_PaymentType_Via_Config()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Create_PaymentType_Via_Instance()
        {
            var instance = new InvoiceFactoring(Heidelpay);
            ConfigurePaymentType(instance);
            var result = await Heidelpay.CreatePaymentTypeAsync(instance);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceFactoring>();
            var charge = await result.ChargeAsync(StandardChargedBasketResult, Currencies.EUR, ShopReturnUri, 
                GetMaximumCustomerSameAddress(GetRandomInvoiceId()), GetMaximumBasket());

            AssertCharge(charge, StandardChargedBasketResult);
            Assert.NotNull(charge?.PaymentId);
        }

        [Fact]
        public async Task Charge_PaymentType_Different_Address()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceFactoring>();
            var ex = await Assert.ThrowsAsync<PaymentException>(() => result.ChargeAsync(10m, Currencies.EUR, ShopReturnUri,
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
