using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class WeChatPayTests : PaymentTypeTestsBase
    {
        private Action<WeChatPay> ConfigurePaymentType { get; } = new Action<WeChatPay>(x =>
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
            var instance = new WeChatPay(Heidelpay);
            ConfigurePaymentType(instance);
            var result = await Heidelpay.CreatePaymentTypeAsync(instance);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var typeInstance = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            var charge = await typeInstance.ChargeAsync(decimal.One, Currencies.EUR, ShopReturnUri);
            AssertCharge(charge, decimal.One, Status.Pending);
            Assert.Equal(typeInstance.Id, charge.TypeId);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var createdTypeInstance = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);

            Assert.NotNull(createdTypeInstance?.Id);

            var fetchedCard = await Heidelpay.FetchPaymentTypeAsync<WeChatPay>(createdTypeInstance.Id);

            Assert.NotNull(fetchedCard?.Id);
        }
    }
}
