using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class GiropayTests : PaymentTypeTestsBase
    {
        private Action<Giropay> ConfigurePaymentType { get; } = new Action<Giropay>(x =>
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
            var instance = new Giropay(Heidelpay);
            ConfigurePaymentType(instance);
            var result = await Heidelpay.CreatePaymentTypeAsync(instance);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Giropay>(ConfigurePaymentType);
            var charge = await Heidelpay.ChargeAsync(decimal.One, Currencies.EUR, result, TestReturnUri);
            AssertCharge(charge, decimal.One, Status.Pending);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Giropay>(ConfigurePaymentType);
            var fetched = await Heidelpay.FetchPaymentTypeAsync<Giropay>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
