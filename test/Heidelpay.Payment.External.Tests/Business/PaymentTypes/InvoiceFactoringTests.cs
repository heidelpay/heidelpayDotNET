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
        public async Task Charge_PaymentType_With_InvoiceId()
        {
            var customer = await Heidelpay.CreateCustomerAsync(CreateFactoringOKCustomer(GetRandomInvoiceId()));

            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceFactoring>();
            var charge = await result.ChargeAsync(StandardChargedBasketResult, Currencies.EUR, ShopReturnUri, customer, GetMaximumBasket(), GetRandomInvoiceId());
            var shipment = await Heidelpay.ShipmentAsync(charge?.PaymentId, GetRandomInvoiceId());

            AssertShipment(shipment);
            Assert.NotNull(shipment.Id);
        }

        [Fact]
        public async Task Shipment_PaymentType_With_InvoiceId()
        {
            var customer = await Heidelpay.CreateCustomerAsync(CreateFactoringOKCustomer(GetRandomInvoiceId()));
            var basket = await Heidelpay.CreateBasketAsync(GetMaximumBasket());
            var paymentType = await Heidelpay.CreatePaymentTypeAsync<InvoiceFactoring>();

            var charge = await Heidelpay.ChargeAsync(StandardChargedBasketResult, Currencies.EUR, paymentType, ShopReturnUri, customer, basket, card3ds:false);
            var shipment = await Heidelpay.ShipmentAsync(charge?.PaymentId, GetRandomInvoiceId());

            Assert.NotNull(charge?.PaymentId);
            AssertShipment(shipment);
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
