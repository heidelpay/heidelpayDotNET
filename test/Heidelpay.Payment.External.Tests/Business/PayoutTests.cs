using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class PayoutTest : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Payout_Card_Minimal()
        {
            var heidelpay = Heidelpay;
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var payout = await heidelpay.PayoutAsync(decimal.One, "EUR", card, new Uri("https://www.heidelpay.com"));

            Assert.NotNull(payout?.Id);

            var payoutFetched = await heidelpay.FetchPayoutAsync(payout.PaymentId, payout.Id);

            Assert.Equal(payout.Amount, payoutFetched.Amount);
            Assert.Equal(payout.BasketId, payoutFetched.BasketId);
            Assert.Equal(payout.Currency, payoutFetched.Currency);
            Assert.Equal(payout.CustomerId, payoutFetched.CustomerId);
            Assert.Equal(payout.Id, payoutFetched.Id);
            Assert.Equal(payout.MetadataId, payoutFetched.MetadataId);
            Assert.Equal(payout.OrderId, payoutFetched.OrderId);
            Assert.Equal(payout.PaymentId, payoutFetched.PaymentId);
            Assert.Equal(payout.PaymentReference, payoutFetched.PaymentReference);
            Assert.Equal(payout.RedirectUrl, payoutFetched.RedirectUrl);
            Assert.Equal(payout.ReturnUrl, payoutFetched.ReturnUrl);
            Assert.Equal(payout.Status, payoutFetched.Status);
            Assert.Equal(payout.TypeId, payoutFetched.TypeId);
        }
    }
}
