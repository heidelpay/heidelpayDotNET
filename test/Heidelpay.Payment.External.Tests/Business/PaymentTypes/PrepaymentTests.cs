using Heidelpay.Payment.PaymentTypes;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class PrepaymentTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Prepayment>();
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Prepayment>();
            var charge = await Heidelpay.ChargeAsync(decimal.One, "EUR", result, TestReturnUri);
            AssertCharge(charge, decimal.One, Status.Pending);
            Assert.NotNull(charge?.ReturnUrl);
            Assert.NotNull(charge?.Processing?.Iban);
            Assert.NotNull(charge?.Processing?.Bic);
            Assert.NotNull(charge?.Processing?.Descriptor);
            Assert.NotNull(charge?.Processing?.Holder);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Prepayment>();
            var fetched = await Heidelpay.FetchPaymentTypeAsync<Prepayment>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
