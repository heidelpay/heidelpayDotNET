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
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<Ideal>(x => x.Bic = "RABONL2U");
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<Ideal>(x => x.Bic = "RABONL2U");
            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "EUR", result, TestReturnUri);
            Assert.NotNull(charge?.Id);
            Assert.NotNull(charge?.RedirectUrl);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync<Ideal>(x => x.Bic = "RABONL2U");
            var fetched = await BuildHeidelpay().FetchPaymentTypeAsync<Ideal>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
