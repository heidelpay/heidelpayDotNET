using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class ChargeAfterAuthorizationTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Fetch_Authorization()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card) { Amount = 1m, Currency = "EUR", ReturnUrl = TestReturnUri });
            var fetched = await Heidelpay.FetchAuthorizationAsync(auth.PaymentId);

            Assert.NotNull(fetched?.Id);
        }

        [Fact]
        public async Task Full_Charge_After_Authorization()
        {
            var orderId = GetRandomId();

            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card) { Amount = 1m, Currency = "EUR", ReturnUrl = TestReturnUri, OrderId = orderId });
            var fetched = await Heidelpay.FetchAuthorizationAsync(auth.PaymentId);
            var charge = await fetched.ChargeAsync();

            Assert.NotNull(charge?.Id);
            Assert.Equal(orderId, auth.OrderId);
            Assert.Equal(orderId, fetched.OrderId);
            Assert.Equal(orderId, charge.OrderId);
        }

        [Fact]
        public async Task Full_Charge_After_Authorization_Heidelpay()
        {
            var orderId = GetRandomId();

            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card) { Amount = 1m, Currency = "EUR", ReturnUrl = TestReturnUri, OrderId = orderId });
            var charge = await Heidelpay.ChargeAuthorizationAsync(auth.PaymentId);

            Assert.NotNull(charge?.Id);
            Assert.Equal(orderId, auth.OrderId);
            Assert.Equal(orderId, charge.OrderId);
        }

        [Fact]
        public async Task Partial_Charge_After_Authorization()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card) { Amount = 1m, Currency = "EUR", ReturnUrl = TestReturnUri });

            var charge = await auth.ChargeAsync(0.1m);

            Assert.NotNull(charge?.Id);
            Assert.Equal(0.1m, charge.Amount);
        }
    }
}
