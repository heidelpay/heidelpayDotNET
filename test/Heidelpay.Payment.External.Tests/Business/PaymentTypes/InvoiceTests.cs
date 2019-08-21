using Heidelpay.Payment.PaymentTypes;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class InvoiceTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Invoice>();
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Invoice>();
            var charge = await Heidelpay.ChargeAsync(decimal.One, "EUR", result, ShopReturnUri);
            AssertCharge(charge, decimal.One, Status.Pending);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Invoice>();
            var fetched = await Heidelpay.FetchPaymentTypeAsync<Invoice>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
