using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class SofortTests : PaymentTypeTestsBase
    {
        private Action<Sofort> ConfigurePaymentType { get; } = new Action<Sofort>(x =>
        {

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
            var instance = new Sofort(Heidelpay);
            ConfigurePaymentType(instance);
            var result = await Heidelpay.CreatePaymentTypeAsync(instance);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Sofort>();
            var charge = await Heidelpay.ChargeAsync(decimal.One, Currencies.EUR, result, TestReturnUri);
            AssertCharge(charge, decimal.One, Status.Pending);
            Assert.NotNull(charge?.RedirectUrl);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Sofort>();
            var fetched = await Heidelpay.FetchPaymentTypeAsync<Sofort>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
