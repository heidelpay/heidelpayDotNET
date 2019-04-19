using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class ApplepayTests : PaymentTypeTestsBase
    {
        private Action<Applepay> ConfigurePaymentType { get; } = new Action<Applepay>(x =>
        {
            
        });

        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            Assert.NotNull(result?.Id);
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
