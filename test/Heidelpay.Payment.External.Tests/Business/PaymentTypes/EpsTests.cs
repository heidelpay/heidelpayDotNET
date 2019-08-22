using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class EpsTests : PaymentTypeTestsBase
    {
        private Action<Eps> ConfigurePaymentType { get; } = new Action<Eps>(x =>
        {
            x.Bic = "SPFKAT2BXXX";
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
            var instance = new Eps(Heidelpay);
            ConfigurePaymentType(instance);
            var result = await Heidelpay.CreatePaymentTypeAsync(instance);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Create_Eps_With_BicType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_Eps_Type()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Eps>();
            var charge = await Heidelpay.ChargeAsync(decimal.One, Currencies.EUR, result, TestReturnUri);
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
