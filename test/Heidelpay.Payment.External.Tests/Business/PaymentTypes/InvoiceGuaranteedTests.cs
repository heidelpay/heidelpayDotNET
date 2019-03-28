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
        public async Task Authorize_PaymentType()
        {
            var paymentType = await BuildHeidelpay().CreatePaymentTypeAsync(new InvoiceGuaranteed());
            var authorization = await paymentType.AuthorizeAsync(decimal.One, "EUR", new Uri("https://www.meinShop.de"), GetMaximumCustomerSameAddress(GetRandomId()));
            Assert.NotNull(authorization?.Id);
        }

        [Fact]
        public async Task Authorize_PaymentType_Different_Address()
        {
            var paymentType = await BuildHeidelpay().CreatePaymentTypeAsync(new InvoiceGuaranteed());

            await Assert.ThrowsAsync<PaymentException>(() => paymentType.AuthorizeAsync(decimal.One, "EUR", new Uri("https://www.meinShop.de"), GetMaximumCustomer(GetRandomId())));
        }

        [Fact]
        public async Task Shipment_PaymentType()
        {
            var auth = await BuildHeidelpay().AuthorizeAsync(decimal.One, "EUR", new InvoiceGuaranteed(), new Uri("https://www.meinShop.de"), GetMaximumCustomerSameAddress(GetRandomId()));
            var shipment = await BuildHeidelpay().ShipmentAsync(auth);
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
