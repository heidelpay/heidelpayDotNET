using Heidelpay.Payment.PaymentTypes;
using System.Threading.Tasks;
using Xunit;


namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class EpsTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_Eps()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Eps>();
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Create_Eps_With_BicType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Eps>(x =>  x.Bic = "SPFKAT2BXXX");
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_Eps_Type()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Eps>();
            var charge = await Heidelpay.ChargeAsync(decimal.One, "EUR", result, TestReturnUri);
            AssertCharge(charge, decimal.One, Status.Pending);
        }

        [Fact]
        public async Task Fetch_Eps_Type()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Eps>();
            var fetchedEps = await Heidelpay.FetchPaymentTypeAsync<Eps>(result.Id);
            Assert.NotNull(fetchedEps?.Id);
        }
    }
}
