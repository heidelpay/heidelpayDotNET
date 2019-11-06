using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class InvoiceGuaranteedTests : PaymentTypeTestsBase
    {
        private Action<InvoiceGuaranteed> ConfigurePaymentType { get; } = new Action<InvoiceGuaranteed>(x =>
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
            var instance = new InvoiceGuaranteed(Heidelpay);
            ConfigurePaymentType(instance);
            var result = await Heidelpay.CreatePaymentTypeAsync(instance);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceGuaranteed>();
            var charge = await result.ChargeAsync(10.0m, Currencies.EUR, ShopReturnUri, GetMaximumCustomerSameAddress(GetRandomId()));
            AssertCharge(charge, 10m, Status.Pending);
        }

        [Fact]
        public async Task Authorize_PaymentType_Different_Address()
        {
            var paymentType = await Heidelpay.CreatePaymentTypeAsync<InvoiceGuaranteed>();
            await Assert.ThrowsAsync<PaymentException>(() => paymentType.ChargeAsync(decimal.One, Currencies.EUR, ShopReturnUri, GetMaximumCustomer(GetRandomId())));
        }

        [Fact]
        public async Task Shipment_PaymentType()
        {
            string invoiceId = GetRandomInvoiceId();

            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceGuaranteed>();
            var charge = await result.ChargeAsync(10.0m, Currencies.EUR, ShopReturnUri, GetMaximumCustomerSameAddress(GetRandomId()));
            var shipment = await Heidelpay.ShipmentAsync(charge?.PaymentId, invoiceId);

            AssertCharge(charge, 10m, Status.Pending);
            AssertShipment(shipment);
            Assert.Equal(invoiceId, shipment.InvoiceId);
        }

        [Fact]
        public async Task Charge_PaymentType_WithInvoiceId()
        {
            Basket basket = GetMinimumBasket();
            string invoiceId = GetRandomInvoiceId();

            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceGuaranteed>();
            var charge = await result.ChargeAsync(basket.AmountTotalGross, Currencies.EUR, ShopReturnUri, GetMaximumCustomerSameAddress(GetRandomId()), basket, invoiceId);

            Assert.NotNull(charge);
            Assert.NotNull(charge.PaymentId);
            Assert.Equal(invoiceId, charge.InvoiceId);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceGuaranteed>();
            var fetched = await Heidelpay.FetchPaymentTypeAsync<InvoiceGuaranteed>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
