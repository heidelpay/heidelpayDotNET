using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class CancelAfterChargeTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Fetch_Charge_With_Id()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await Heidelpay.ChargeAsync(1m, Currencies.EUR, card.Id, TestReturnUri, card3ds: false);
            var fetched = await Heidelpay.FetchChargeAsync(charge.PaymentId, charge.Id);

            Assert.NotNull(charge?.Id);

            AssertEquals(charge, fetched);
        }

        [Fact]
        public async Task Full_Refund_With_Id()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await Heidelpay.ChargeAsync(1m, Currencies.EUR, card.Id, TestReturnUri, card3ds: false);
            var cancel = await Heidelpay.CancelChargeAsync(charge.PaymentId, charge.Id);

            Assert.NotNull(cancel?.Id);
        }

        [Fact]
        public async Task Full_Refund_With_Charge()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await Heidelpay.ChargeAsync(1m, Currencies.EUR, card.Id, TestReturnUri, card3ds: false);
            var cancel = await charge.CancelAsync();

            Assert.NotNull(cancel?.Id);
        }

        [Fact]
        public async Task Partial_Refund_With_Id()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await Heidelpay.ChargeAsync(10m, Currencies.EUR, card.Id, TestReturnUri, card3ds: false);
            var cancel = await Heidelpay.CancelChargeAsync(charge.PaymentId, charge.Id, amount: 5.54m);

            Assert.NotNull(cancel?.Id);
            Assert.Equal(5.54m, cancel.Amount);
        }

        [Fact]
        public async Task Partial_Refund_With_Charge()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await Heidelpay.ChargeAsync(10m, Currencies.EUR, card.Id, TestReturnUri, card3ds: false);
            var fetchedCharge = await Heidelpay.FetchChargeAsync(charge.PaymentId, charge.Id);
            var cancel = await fetchedCharge.CancelAsync(amount: 5.54m);

            Assert.NotNull(cancel?.Id);
            Assert.Equal(5.54m, cancel.Amount);

            var fetchedCancel = await Heidelpay.FetchCancelAsync(charge.PaymentId, fetchedCharge.Id, cancel.Id);

            Assert.NotNull(fetchedCancel?.Id);
        }

        [Fact]
        public async Task Exceeding_Amount_Throws_Exception()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await Heidelpay.ChargeAsync(1m, Currencies.EUR, card.Id, TestReturnUri, card3ds: false);
            var fetchedCharge = await Heidelpay.FetchChargeAsync(charge.PaymentId, charge.Id);
            var ex = await Assert.ThrowsAsync<PaymentException>(() => fetchedCharge.CancelAsync(amount: 1.01m));
        }
    }
}
