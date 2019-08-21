using Heidelpay.Payment.PaymentTypes;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class IdealTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Ideal>(x => x.Bic = "RABONL2U");
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Ideal>(x => x.Bic = "RABONL2U");
            var charge = await Heidelpay.ChargeAsync(decimal.One, "EUR", result, TestReturnUri);
            AssertCharge(charge, decimal.One, Status.Pending);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Ideal>(x => x.Bic = "RABONL2U");
            var fetched = await Heidelpay.FetchPaymentTypeAsync<Ideal>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
