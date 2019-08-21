using Heidelpay.Payment.PaymentTypes;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class Przelewy24Tests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Przelewy24>();
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Przelewy24>();
            var charge = await Heidelpay.ChargeAsync(decimal.One, "PLN", result, TestReturnUri);
            AssertCharge(charge, decimal.One, status: Status.Pending, currency: "PLN");
            Assert.NotNull(charge?.RedirectUrl);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Przelewy24>();
            var fetched = await Heidelpay.FetchPaymentTypeAsync<Przelewy24>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
