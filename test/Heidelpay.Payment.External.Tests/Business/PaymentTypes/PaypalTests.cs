using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class PaypalTests : PaymentTypeTestsBase
    {
        private Action<Paypal> ConfigurePaymentType { get; } = new Action<Paypal>(x =>
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
            var instance = new Paypal(Heidelpay);
            ConfigurePaymentType(instance);
            var result = await Heidelpay.CreatePaymentTypeAsync(instance);
            Assert.NotNull(result?.Id);
        }


        [Fact]
        public async Task Authorize_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Paypal>();
            var auth = await Heidelpay.AuthorizeAsync(decimal.One, "EUR", result, ShopReturnUri);
            Assert.NotNull(result?.Id);
            AssertAuthorizationSimple(auth, decimal.One, Status.Pending);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Paypal>();
            var charge = await Heidelpay.ChargeAsync(decimal.One, "EUR", result, TestReturnUri);
            AssertCharge(charge, decimal.One, Status.Pending);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Paypal>();
            var fetched = await Heidelpay.FetchPaymentTypeAsync<Paypal>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
