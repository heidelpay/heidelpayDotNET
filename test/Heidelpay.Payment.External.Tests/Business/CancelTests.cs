using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class CancelTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Fetch_Cancel_Authorization_With_Heidelpay()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card) { Amount = 1m, Currency = "EUR", ReturnUrl = TestReturnUri });
            var cancel = await auth.CancelAsync();
            var fetched = await Heidelpay.FetchCancelAsync(auth.PaymentId, cancel.Id);

            Assert.NotNull(fetched?.Id);
            Assert.Equal("COR.000.100.112", fetched.Message.Code);
            Assert.NotNull(fetched.Message.Customer);

            AssertEquals(cancel, fetched);
        }

        [Fact]
        public async Task Fetch_Cancel_Authorization_With_Payment()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card) { Amount = 1m, Currency = "EUR", ReturnUrl = TestReturnUri });
            var cancel = await auth.CancelAsync();

            Assert.Equal("COR.000.100.112", cancel.Message.Code);
            Assert.NotNull(cancel.Message.Customer);

            var cancelList = cancel.Payment.CancelList;

            Assert.Single(cancelList);
            AssertEquals(cancel, cancelList.First());
        }

        [Fact]
        public async Task Fetch_Cancel_Charge_With_Heidelpay()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var charge = await Heidelpay.ChargeAsync(1m, "EUR", card, TestReturnUri);
            var cancel = await charge.CancelAsync();
            var fetched = await Heidelpay.FetchCancelAsync(charge.PaymentId, cancel.Id);

            Assert.NotNull(fetched?.Id);
            Assert.Equal("COR.000.100.112", fetched.Message.Code);
            Assert.NotNull(fetched.Message.Customer);

            AssertEquals(cancel, fetched);
        }

        [Fact]
        public async Task Fetch_Cancel_Charge_With_Payment()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var charge = await Heidelpay.ChargeAsync(1m, "EUR", card, TestReturnUri);
            var cancel = await charge.CancelAsync();

            Assert.Equal("COR.000.100.112", cancel.Message.Code);
            Assert.NotNull(cancel.Message.Customer);

            var cancelList = cancel.Payment.CancelList;

            Assert.Single(cancelList);
            AssertEquals(cancel, cancelList.First());
        }
    }
}
