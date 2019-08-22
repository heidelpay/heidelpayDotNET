using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class PrepaymentTests : PaymentTypeTestsBase
    {
        private Action<Prepayment> ConfigurePaymentType { get; } = new Action<Prepayment>(x =>
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
            var instance = new Prepayment(Heidelpay);
            ConfigurePaymentType(instance);
            var result = await Heidelpay.CreatePaymentTypeAsync(instance);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Prepayment>();
            var charge = await Heidelpay.ChargeAsync(decimal.One, "EUR", result, TestReturnUri);
            AssertCharge(charge, decimal.One, Status.Pending);
            Assert.NotNull(charge?.ReturnUrl);
            Assert.NotNull(charge?.Processing?.Iban);
            Assert.NotNull(charge?.Processing?.Bic);
            Assert.NotNull(charge?.Processing?.Descriptor);
            Assert.NotNull(charge?.Processing?.Holder);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Prepayment>();
            var fetched = await Heidelpay.FetchPaymentTypeAsync<Prepayment>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
