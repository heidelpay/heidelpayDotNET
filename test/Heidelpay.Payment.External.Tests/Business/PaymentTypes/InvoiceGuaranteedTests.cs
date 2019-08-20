using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class InvoiceGuaranteedTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceGuaranteed>();
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceGuaranteed>();
            var charge = await result.ChargeAsync(10.0m, "EUR", ShopReturnUri, GetMaximumCustomerSameAddress(GetRandomId()));
            Assert.NotNull(result?.Id);
            Assert.NotNull(charge?.Id);
        }

        [Fact]
        public async Task Authorize_PaymentType_Different_Address()
        {
            var paymentType = await Heidelpay.CreatePaymentTypeAsync<InvoiceGuaranteed>();
            await Assert.ThrowsAsync<PaymentException>(() => paymentType.ChargeAsync(decimal.One, "EUR", ShopReturnUri, GetMaximumCustomer(GetRandomId())));
        }

        [Fact]
        public async Task Shipment_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<InvoiceGuaranteed>();
            var charge = await result.ChargeAsync(10.0m, "EUR", ShopReturnUri, GetMaximumCustomerSameAddress(GetRandomId()));
            var shipment = await Heidelpay.ShipmentAsync(charge?.PaymentId, Guid.NewGuid().ToString().Substring(0, 15));

            Assert.NotNull(shipment?.Id);
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
