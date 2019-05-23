using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class AlipayTests : PaymentTypeTestsBase
    {
        private Action<Alipay> ConfigurePaymentType { get; } = new Action<Alipay>(x =>
        {
            
        });

        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var typeInstance = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            var charge = await typeInstance.ChargeAsync(decimal.One, "EUR", ShopReturnUri);
            Assert.NotNull(charge?.Id);
            Assert.Equal(decimal.One, charge.Amount);
            Assert.Equal("EUR", charge.Currency);
            Assert.Equal(typeInstance.Id, charge.TypeId);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var createdTypeInstance = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);

            Assert.NotNull(createdTypeInstance?.Id);

            var fetchedCard = await Heidelpay.FetchPaymentTypeAsync<Alipay>(createdTypeInstance.Id);

            Assert.NotNull(fetchedCard?.Id);
        }
    }
}
