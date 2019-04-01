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
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new InvoiceGuaranteed());
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new InvoiceGuaranteed());
            var charge = await result.ChargeAsync(10.0m, "EUR", new Uri("https://www.meinShop.de"), GetMaximumCustomerSameAddress(GetRandomId()));
            Assert.NotNull(result?.Id);
            Assert.NotNull(charge?.Id);
        }

        [Fact]
        public async Task Authorize_PaymentType_Different_Address()
        {
            var paymentType = await BuildHeidelpay().CreatePaymentTypeAsync(new InvoiceGuaranteed());

            await Assert.ThrowsAsync<PaymentException>(() => paymentType.ChargeAsync(decimal.One, "EUR", new Uri("https://www.meinShop.de"), GetMaximumCustomer(GetRandomId())));
        }

        [Fact(Skip="Test fails every other execution because of unspecified provider errors")]
        public async Task Shipment_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new InvoiceGuaranteed());
            var charge = await result.ChargeAsync(10.0m, "EUR", new Uri("https://www.meinShop.de"), GetMaximumCustomerSameAddress(GetRandomId()));
            var shipment = await BuildHeidelpay().ShipmentAsync(charge?.PaymentId);

            Assert.NotNull(shipment?.Id);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(new Giropay());
            var fetched = await BuildHeidelpay().FetchPaymentTypeAsync<Giropay>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
